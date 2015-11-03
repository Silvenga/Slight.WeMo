using System;

namespace Slight.WeMo.Framework.Discovery
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Mono.Upnp;

    using Slight.WeMo.Framework.Models;

    public sealed class WeMoDiscoverer
    {
        public static WeMoDiscoverer Instance { get; } = new WeMoDiscoverer();

        public bool Searching { get; private set; }

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

            Task.Run(() => StartSearchLoop());
        }

        private void StartSearchLoop()
        {
            while (true)
            {
                try
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

                    RemoveOldDevices();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void OnDeviceDetected(DeviceAnnouncement device)
        {
            lock (Devices)
            {
                var location = device.Locations.FirstOrDefault();
                WeMoDevice weMoDevice;
                var success = WeMoDevice.TryCreate(location, out weMoDevice);

                var newDevice = success
                                && !Devices.ContainsKey(device.Udn);

                if (newDevice)
                {
                    Devices.Add(device.Udn, weMoDevice);
                    Console.WriteLine($"Found new WeMo: {weMoDevice.FriendlyName} @ {weMoDevice.Host}");

                    return;
                }

                var updatedDevice = success
                                    && Devices.ContainsKey(device.Udn)
                                    && Devices[device.Udn].Location != location;

                if (updatedDevice)
                {
                    Devices[device.Udn] = weMoDevice;
                    Console.WriteLine($"Found updated WeMo: {weMoDevice.FriendlyName} @ {weMoDevice.Host}");
                    return;
                }

                var existingDevice = success
                                     && Devices.ContainsKey(device.Udn);

                if (existingDevice)
                {
                    Devices[device.Udn].LastDetected = DateTime.Now;
                    Console.WriteLine($"Found existing WeMo: {weMoDevice.FriendlyName} @ {weMoDevice.Host}");
                }
            }
        }

        private void RemoveOldDevices()
        {
            lock (Devices)
            {
                var removedDevices = Devices
               .Where(x => x.Value.LastDetected > DateTime.Now.AddMinutes(5))
               .Select(x => x.Key)
               .ToList();

                foreach (var deviceId in removedDevices)
                {
                    Devices.Remove(deviceId);
                }
            }
        }

        [CanBeNull]
        public WeMoDevice Get([CanBeNull] string deviceId)
        {
            lock (Devices)
            {
                if (deviceId == null)
                {
                    return null;
                }

                WeMoDevice value;
                return Devices.TryGetValue(deviceId, out value)
                    ? value
                    : null;
            }
        }

        [NotNull]
        public IEnumerable<WeMoDevice> GetAll()
        {
            lock (Devices)
            {
                return Devices
                    .Values
                    .ToList();
            }
        }
    }
}
