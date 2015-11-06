namespace Slight.WeMo.Framework.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slight.WeMo.DataAccess;
    using Slight.WeMo.Entities.Models;
    using Slight.WeMo.Framework.Actors;

    public class StateProvider
    {
        public void RecordDeviceState(string deviceId)
        {
            using (var context = new WeMoContext())
            {
                var device = context.WeMoDevices.First(x => x.DeviceId == deviceId);

                var client = new WeMoClient(device);
                var binaryState = client.GetBinaryState();

                var state = new WeMoDeviceState
                {
                    Device = device,
                    CurrentState = binaryState,
                    Timestamp = DateTime.Now
                };

                context.WeMoStates.Add(state);
                context.SaveChanges();
            }
        }

        public IEnumerable<WeMoDeviceState> GetAll()
        {
            using (var context = new WeMoContext())
            {
                return context.WeMoStates
                    .ToList();
            }
        }

        public IEnumerable<WeMoDeviceState> GetByDeviceId(string deviceId)
        {
            using (var context = new WeMoContext())
            {
                return context.WeMoStates
                    .Where(x => x.Device.DeviceId == deviceId)
                    .ToList();
            }
        }
    }
}
