namespace Slight.WeMo.Framework.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Xml.Linq;

    using JetBrains.Annotations;

    using Slight.WeMo.Entities.Models;

    public class WeMoClient
    {
        public const int Delay = 10; // Prevents 500's on WeMo's side

        public WeMoDevice Device { get; }

        public string Address { get; }

        public WeMoClient([NotNull] WeMoDevice device)
        {
            Device = device;
            var address = $"http://{device.Host}:{device.Port}";
            Address = address;
        }

        public void EnumerateDeviceInfo()
        {
            var info = ExecuteDeviceInfo();
            Device.FriendlyName = info.friendlyName;
            Device.ModelName = info.modelName;
            Device.ModelNumber = info.modelNumber;
            Device.SerialNumber = info.serialNumber;
            Device.MacAddress = info.macAddress;
            Device.FirmwareVersion = info.firmwareVersion;
            Device.DeviceId = info.UDN;
            Device.DeviceType = info.deviceType;
        }

        public string GetSignalStrength()
        {
            var response = ExecuteSoapAction("GetSignalStrength");
            return response.GetSignalStrengthResponse.SignalStrength;
        }

        public string GetBinaryState()
        {
            var response = ExecuteSoapAction("GetBinaryState");
            return response.GetBinaryStateResponse.BinaryState;
        }

        public string SetBinaryState(string state)
        {
            var response = ExecuteSoapAction("SetBinaryState", $"<BinaryState>{state}</BinaryState>").SetBinaryStateResponse;
            var results = (IDictionary<string, object>) response;
            return results.ContainsKey("CountdownEndTime") ? response.CountdownEndTime : response.BinaryState;
        }

        public string ChangeFriendlyName(string name)
        {
            ExecuteSoapAction("ChangeFriendlyName", $"<FriendlyName>{name}</FriendlyName>");

            try
            {
                EnumerateDeviceInfo();
                return Device.FriendlyName;
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        private dynamic ExecuteSoapAction(string command, string data = "")
        {
            var client = CreateClient();
            var request = CreateBasicEventRequest(command, data);

            var response = client.SendAsync(request).Result;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            var content = ParseResponse(response.Content.ReadAsStringAsync().Result).Envelope.Body;

            Thread.Sleep(Delay);

            return content;
        }

        private HttpRequestMessage CreateBasicEventRequest(string command, string data)
        {
            var body = BaseBody(command, data);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{Address}/upnp/control/basicevent1");
            request.Headers.Add("SOAPACTION", $"\"urn:Belkin:service:basicevent:1#{command}\"");
            request.Content = new StringContent(body, Encoding.UTF8, "text/xml");

            return request;
        }

        private dynamic ExecuteDeviceInfo()
        {
            var client = CreateClient();
            var request = CreateBasicInfoRequest();
            var response = client.SendAsync(request).Result;

            var content = ParseResponse(response.Content.ReadAsStringAsync().Result).root.device;

            Thread.Sleep(Delay);

            return content;
        }

        private HttpRequestMessage CreateBasicInfoRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Address}/setup.xml");
            return request;
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            return client;
        }

        private static string BaseBody(string command, string data)
        {
            var body =
             $@"<?xml version='1.0' encoding='utf-8'?>
                <s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/' s:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
                    <s:Body>
                        <u:{command} xmlns:u='urn:Belkin:service:basicevent:1'>
                            {data}
                        </u:{command}>
                    </s:Body>
                </s:Envelope>
                ";
            return body;
        }

        private static dynamic ParseResponse(string body)
        {
            var doc = XDocument.Parse(body);

            dynamic root = new ExpandoObject();
            XmlToDynamic.Parse(root, doc.Elements().First());

            return root;
        }
    }
}
