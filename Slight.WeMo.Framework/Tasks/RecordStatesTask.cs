namespace Slight.WeMo.Framework.Tasks
{
    using System.Linq;

    using Slight.WeMo.Framework.Providers;

    public class RecordStatesTask
    {
        private readonly DeviceProvider _deviceProvider = new DeviceProvider();
        private readonly StateProvider _stateProvider = new StateProvider();

        public void RecordState()
        {
            foreach (var device in _deviceProvider.GetAll()
                .Where(x => !x.Disabled)
                .Select(x => x.DeviceId))
            {
                _stateProvider.RecordDeviceState(device);
            }
        }
    }
}
