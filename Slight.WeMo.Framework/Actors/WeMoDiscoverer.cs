namespace Slight.WeMo.Framework.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Mono.Upnp;

    using Slight.WeMo.Framework.Providers;

    public sealed class WeMoDiscoverer
    {
        private readonly DeviceProvider _provider = new DeviceProvider();

        public static WeMoDiscoverer Instance { get; } = new WeMoDiscoverer();

        public bool Searching { get; private set; }

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

            Task.Run(() => StartSearchLoop());
        }

        private void StartSearchLoop()
        {
            while (true)
            {
                Console.WriteLine("Launching search.");
                var list = new List<DeviceAnnouncement>();

                using (var client = new Client())
                {
                    client.BrowseAll();
                    client.DeviceAdded += (sender, args) =>
                    {
                        list.Add(args.Device);
                    };

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }

                Console.WriteLine($"Search completed. Found {list.Count} devices.");

                foreach (var announcement in list)
                {
                    OnDeviceDetected(announcement);
                }

                _provider.RemoveOldDevices();

                Thread.Sleep(SearchDelay());
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void OnDeviceDetected(DeviceAnnouncement device)
        {
            var location = device.Locations.FirstOrDefault();

            var success = _provider.IsWeMoDevice(location);

            if (!success)
            {
                return;
            }

            var newDevice = !_provider.Exists(device.Udn);

            var updatedDevice = _provider.Exists(device.Udn)
                               && _provider.Get(device.Udn).Location != location;

            var existingDevice = _provider.Exists(device.Udn);

            if (newDevice)
            {
                var weMoDevice = _provider.Create(location);
                Console.WriteLine($"Found new WeMo: {weMoDevice.FriendlyName} @ {weMoDevice.Host}");
            }
            else if (updatedDevice)
            {
                var weMoDevice = _provider.Update(device.Udn, location);
                Console.WriteLine($"Found updated WeMo: {weMoDevice.FriendlyName} @ {weMoDevice.Host}");
            }
            else if (existingDevice)
            {
                var weMoDevice = _provider.Update(device.Udn, location);
                Console.WriteLine($"Found existing WeMo: {weMoDevice.FriendlyName} @ {weMoDevice.Host}");
            }
        }

        private TimeSpan SearchDelay()
        {
            return _provider.Any()
                ? TimeSpan.FromMinutes(1)
                : TimeSpan.FromSeconds(5);
        }
    }
}
