namespace Slight.WeMo.Framework.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Xml.Linq;

    public class WeMoClient
    {
        public string Address { get; }

        public WeMoClient(string host, int port)
        {
            var address = $"http://{host}:{port}";
            Address = address;
        }

        public dynamic ExecuteSoapAction(string command, string data = "")
        {
            var client = CreateClient();
            var request = CreateBasicEventRequest(command, data);

            var response = client.SendAsync(request).Result;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            var content = ParseResponse(response.Content.ReadAsStringAsync().Result).Envelope.Body;
            return ((IDictionary<string, object>) content)[$"{command}Response"];
        }

        private HttpRequestMessage CreateBasicEventRequest(string command, string data)
        {
            var body = BaseBody(command, data);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{Address}/upnp/control/basicevent1");
            request.Headers.Add("SOAPACTION", $"\"urn:Belkin:service:basicevent:1#{command}\"");
            request.Content = new StringContent(body, Encoding.UTF8, "text/xml");

            return request;
        }

        public dynamic ExecuteDeviceInfo()
        {
            var client = CreateClient();
            var request = CreateBasicInfoRequest();
            var response = client.SendAsync(request).Result;

            var content = ParseResponse(response.Content.ReadAsStringAsync().Result).root.device;
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
