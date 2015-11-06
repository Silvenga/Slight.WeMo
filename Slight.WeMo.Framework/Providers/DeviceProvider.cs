namespace Slight.WeMo.Framework.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using Slight.WeMo.DataAccess;
    using Slight.WeMo.Entities.Models;
    using Slight.WeMo.Framework.Actors;

    public class DeviceProvider
    {
        public WeMoDevice Create(string location)
        {
            using (var context = new WeMoContext())
            {
                var url = new Uri(location);
                var device = new WeMoDevice
                {
                    Host = url.Host,
                    Port = url.Port,
                    Location = location
                };

                var client = new WeMoClient(device);
                client.EnumerateDeviceInfo();

                context.WeMoDevices
                    .Add(device);
                context.SaveChanges();

                return device;
            }
        }

        public WeMoDevice Update(string deviceId, string location)
        {
            using (var context = new WeMoContext())
            {
                var url = new Uri(location);
                var device = context.WeMoDevices
                    .First(x => x.DeviceId == deviceId);

                device.Host = url.Host;
                device.Port = url.Port;
                device.Location = location;
                device.Disabled = false;

                var client = new WeMoClient(device);
                client.EnumerateDeviceInfo();

                context.SaveChanges();

                return device;
            }
        }

        public void Remove(string deviceId)
        {
            using (var context = new WeMoContext())
            {
                var device = context.WeMoDevices
                    .First(x => x.DeviceId == deviceId);
                context.Remove(device);
                context.SaveChanges();
            }
        }

        public void DisableOldDevices()
        {
            using (var context = new WeMoContext())
            {
                var oldCutoff = DateTime.Now.AddMinutes(5);
                var removedDevices = context.WeMoDevices
                    .Where(x => x.LastDetected > oldCutoff);

                foreach (var device in removedDevices)
                {
                    device.Disabled = true;
                }

                context.WeMoDevices
                    .UpdateRange(removedDevices);
                context.SaveChanges();
            }

        }

        public WeMoDevice Get(string deviceId)
        {
            using (var context = new WeMoContext())
            {
                var device = context.WeMoDevices
                    .FirstOrDefault(x => x.DeviceId == deviceId);
                return device;
            }
        }

        public IEnumerable<WeMoDevice> GetAll()
        {
            using (var context = new WeMoContext())
            {
                var device = context.WeMoDevices
                    .ToList();
                return device;
            }
        }

        public bool Exists([CanBeNull] string deviceId)
        {
            using (var context = new WeMoContext())
            {
                var exists = !string.IsNullOrEmpty(deviceId)
                             && context.WeMoDevices.Any(x => x.DeviceId == deviceId);
                return exists;
            }
        }

        public bool Any()
        {
            using (var context = new WeMoContext())
            {
                var any = context.WeMoDevices
                    .Any();
                return any;
            }
        }

        public bool IsWeMoDevice(string location)
        {
            Uri uri;
            var looksLikeWeMo = location != null
                                && Uri.TryCreate(location, UriKind.RelativeOrAbsolute, out uri)
                                && uri.PathAndQuery == "/setup.xml"
                                && uri.Port > 49150
                                && uri.Port < 49160;
            return looksLikeWeMo;
        }
    }
}
