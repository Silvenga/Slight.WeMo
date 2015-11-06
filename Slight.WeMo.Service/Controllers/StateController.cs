namespace Slight.WeMo.Service.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Slight.WeMo.Entities.Models;
    using Slight.WeMo.Framework.Providers;

    /// <summary>
    /// Get recorded WeMo device states.
    /// </summary>
    [RoutePrefix("")]
    public class StateController : ApiController
    {
        private readonly DeviceProvider _deviceProvider = new DeviceProvider();
        private readonly StateProvider _stateProvider = new StateProvider();

        /// <summary>
        /// Get all recorded WeMo states.
        /// </summary>
        /// <returns>A WeMo device.</returns>
        /// <response code="200">OK</response>
        [ResponseType(typeof(IEnumerable<WeMoDeviceState>))]
        [Route("wemo/states"), HttpGet]
        public IHttpActionResult GetStates()
        {
            var states = _stateProvider.GetAll();
            return Ok(states);
        }

        /// <summary>
        /// Get a recorded WeMo device's states.
        /// </summary>
        /// <returns>A WeMo device.</returns>
        /// <response code="200">OK</response>
        [ResponseType(typeof(IEnumerable<WeMoDeviceState>))]
        [Route("wemo/devices/{deviceId}/states"), HttpGet]
        public IHttpActionResult GetDeviceStates(string deviceId)
        {
            if (!_deviceProvider.Exists(deviceId))
            {
                return NotFound();
            }

            var states = _stateProvider.GetByDeviceId(deviceId);
            return Ok(states);
        }
    }
}
