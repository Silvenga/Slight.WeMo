namespace Slight.WeMo.Service.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Slight.WeMo.Entities.Models;
    using Slight.WeMo.Framework.Actors;
    using Slight.WeMo.Framework.Providers;

    /// <summary>
    /// Manages WeMo devices.
    /// </summary>
    [RoutePrefix("wemo")]
    public class DeviceController : ApiController
    {
        private readonly DeviceProvider _provider = new DeviceProvider();

        /// <summary>
        /// Get a list of all known WeMo devices.
        /// </summary>
        /// <returns>A WeMo device.</returns>
        /// <response code="200">OK</response>
        [ResponseType(typeof(IEnumerable<WeMoDevice>))]
        [Route("devices"), HttpGet]
        public IHttpActionResult ListDevices()
        {
            var deviceList = _provider.GetAll();

            return Ok(deviceList);
        }

        /// <summary>
        /// Get a WeMo device.
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <returns>A WeMo device.</returns>
        /// <response code="200">OK</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(WeMoDevice))]
        [Route("device/{deviceId}"), HttpGet]
        public IHttpActionResult GetDevice(string deviceId)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                return Ok(device);
            }

            return NotFound();
        }

        /// <summary>
        /// Get a WeMo device's WiFi signal strength.
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <returns>The percentage of the device WiFi signal strength.</returns>
        /// <response code="200">OK</response>
        /// /// <response code="400">User error on request.</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(string))]
        [Route("device/{deviceId}/signal"), HttpGet]
        public IHttpActionResult GetSignalStrength(string deviceId)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                var client = new WeMoClient(device);
                var results = client.GetSignalStrength();
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }

        /// <summary>
        /// Force a rediscovery of a WeMo device.
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <returns>The state of the WeMo device.</returns>
        /// <response code="200">OK</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(WeMoDevice))]
        [Route("device/{deviceId}/refresh"), HttpPost]
        public IHttpActionResult EnumerateDevice(string deviceId)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                var client = new WeMoClient(device);
                client.EnumerateDeviceInfo();
                return Ok(device);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the switch state of a WeMo device.
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <returns>The current switch state of the WeMo device.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">User error on request.</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(string))]
        [Route("device/{deviceId}/switch"), HttpGet]
        public IHttpActionResult GetSwitchStatus(string deviceId)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                var client = new WeMoClient(device);
                var results = client.GetBinaryState();
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }

        /// <summary>
        /// Set the switch state of a WeMo device to off.
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <returns>The current switch state of the WeMo device.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">User error on request.</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(string))]
        [Route("device/{deviceId}/switch/off"), HttpPut]
        public IHttpActionResult SetOff(string deviceId)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                var client = new WeMoClient(device);
                var results = client.SetBinaryState("0");
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }

        /// <summary>
        /// Set the switch state of a WeMo device to on.
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <returns>The current switch state of the WeMo device.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">User error on request.</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(string))]
        [Route("device/{deviceId}/switch/on"), HttpPut]
        public IHttpActionResult SetOn(string deviceId)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                var client = new WeMoClient(device);
                var results = client.SetBinaryState("1");
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }

        /// <summary>
        /// Set the name of a WeMo device. 
        /// </summary>
        /// <param name="deviceId">The id of the device .</param>
        /// <param name="name">The name to change to.</param>
        /// <returns>The current name of the WeMo device.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">User error on request.</response>
        /// <response code="404">Device cannot be found.</response>
        [ResponseType(typeof(string))]
        [Route("device/{deviceId}/name"), HttpPut]
        public IHttpActionResult SetName(string deviceId, [FromBody] string name)
        {
            var device = _provider.Get(deviceId);

            if (device != null)
            {
                var client = new WeMoClient(device);
                var results = client.ChangeFriendlyName(name);
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }
    }
}
