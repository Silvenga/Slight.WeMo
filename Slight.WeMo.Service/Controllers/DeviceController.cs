namespace Slight.WeMo.Service.Controllers
{
    using System.Web.Http;

    using Slight.WeMo.Framework.Discovery;

    [RoutePrefix("wemo")]
    public class DeviceController : ApiController
    {
        [Route("devices"), HttpGet]
        public IHttpActionResult ListDevices()
        {
            var deviceList = WeMoDiscoverer.Instance
                .GetAll();

            return Ok(deviceList);
        }

        [Route("device/{deviceId}"), HttpGet]
        public IHttpActionResult GetDevice(string deviceId)
        {
            var device = WeMoDiscoverer.Instance.Get(deviceId);

            if (device != null)
            {
                return Ok(device);
            }

            return NotFound();
        }

        [Route("device/{deviceId}/switch"), HttpGet]
        public IHttpActionResult GetSwitchStatus(string deviceId)
        {
            var device = WeMoDiscoverer.Instance.Get(deviceId);

            if (device != null)
            {
                var results = device.GetBinaryState();
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }

        [Route("device/{deviceId}/switch/off"), HttpPut]
        public IHttpActionResult SetOff(string deviceId)
        {
            var device = WeMoDiscoverer.Instance.Get(deviceId);

            if (device != null)
            {
                var results = device.SetBinaryState("0");
                if (results == "Error")
                {
                    return BadRequest();
                }
                return Ok(results);
            }

            return NotFound();
        }

        [Route("device/{deviceId}/switch/on"), HttpPut]
        public IHttpActionResult SetOn(string deviceId)
        {
            var device = WeMoDiscoverer.Instance.Get(deviceId);

            if (device != null)
            {
                var results = device.SetBinaryState("1");
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
