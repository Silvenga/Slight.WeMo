using System;

namespace Slight.WeMo.Framework.Discovery
{
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using Mono.Upnp;

    using Slight.WeMo.Framework.Models;

    public sealed class WeMoDiscoverer
    {
        public static WeMoDiscoverer Instance { get; } = new WeMoDiscoverer();

        public bool Searching { get; private set; }

        private Client UpnpClient { get; } = new Client();

        private Dictionary<string, WeMoDevice> Devices { get; } = new Dictionary<string, WeMoDevice>();

        private WeMoDiscoverer()
        {
        }

        public void StartSearch()
        {
            if (Searching)
            {
                throw new Exception("Already searching.");
            }
            Searching = true;

            UpnpClient.BrowseAll();
            UpnpClient.DeviceAdded += UpnpClientOnDeviceAdded;
            UpnpClient.DeviceRemoved += UpnpClientOnDeviceRemoved;
        }

        private void UpnpClientOnDeviceAdded(object sender, DeviceEventArgs deviceEventArgs)
        {
            var device = deviceEventArgs.Device;
            WeMoDevice weMoDevice;
            var success = WeMoDevice.TryCreate(device.Locations.FirstOrDefault(), out weMoDevice);
            if (success)
            {
                Devices.Add(device.Udn, weMoDevice);
                Console.WriteLine($"Found device: {weMoDevice.FriendlyName} {weMoDevice.DeviceId}");
                Console.WriteLine();
            }
        }

        private void UpnpClientOnDeviceRemoved(object sender, DeviceEventArgs deviceEventArgs)
        {
            Console.WriteLine($"Device removed: {deviceEventArgs.Device.Udn}.");
            Devices.Remove(deviceEventArgs.Device.Udn);
        }

        [CanBeNull]
        public WeMoDevice Get([CanBeNull] string deviceId)
        {
            if (deviceId == null)
            {
                return null;
            }

            WeMoDevice value;
            return Devices.TryGetValue(deviceId, out value) ? value : null;
        }

        [NotNull]
        public IEnumerable<WeMoDevice> GetAll()
        {
            return Devices.Values.ToList();
        }
    }
}
