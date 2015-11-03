namespace Slight.WeMo.Framework.Models
{
    using System;
    using System.Collections.Generic;

    using Slight.WeMo.Framework.Actors;

    public class WeMoDevice
    {
        public string DeviceId { get; set; }

        public string FriendlyName { get; set; }

        public string DeviceType { get; set; }

        public string ModelName { get; set; }

        public string ModelNumber { get; set; }

        public string SerialNumber { get; set; }

        public string MacAddress { get; set; }

        public string FirmwareVersion { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Location { get; set; }

        public DateTime LastDetected { get; set; }

        private WeMoClient Client { get; }

        private WeMoDevice(string host, int port, string location)
        {
            Host = host;
            Port = port;
            Location = location;
            LastDetected = DateTime.Now;

            Client = new WeMoClient(host, port);
            EnumerateDeviceInfo();
        }

        public static bool TryCreate(string location, out WeMoDevice device)
        {
            device = null;

            try
            {
                var url = new Uri(location);
                var looksLikeWeMo = url.PathAndQuery == "/setup.xml"
                    && url.Port > 49150
                    && url.Port < 49160;
                if (!looksLikeWeMo)
                {
                    return false;
                }
                device = new WeMoDevice(url.Host, url.Port, location);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void EnumerateDeviceInfo()
        {
            var info = Client.ExecuteDeviceInfo();
            FriendlyName = info.friendlyName;
            ModelName = info.modelName;
            ModelNumber = info.modelNumber;
            SerialNumber = info.serialNumber;
            MacAddress = info.macAddress;
            FirmwareVersion = info.firmwareVersion;
            DeviceId = info.UDN;
            DeviceType = info.deviceType;
        }

        public string GetSignalStrength()
        {
            var response = Client.ExecuteSoapAction("GetSignalStrength");
            return response.SignalStrength;
        }

        public string GetBinaryState()
        {
            var response = Client.ExecuteSoapAction("GetBinaryState");
            return response.BinaryState;
        }

        public string SetBinaryState(string state)
        {
            var response = Client.ExecuteSoapAction("SetBinaryState", $"<BinaryState>{state}</BinaryState>");
            var results = ((IDictionary<string, object>) response);
            return results.ContainsKey("CountdownEndTime") ? response.CountdownEndTime : response.BinaryState;
        }
    }
}
