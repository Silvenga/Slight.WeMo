using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Slight.WeMo.Service.Startup))]

namespace Slight.WeMo.Service
{
    using System.Web.Http;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
