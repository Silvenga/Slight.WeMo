using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Slight.WeMo.Service.Startup))]

namespace Slight.WeMo.Service
{
    using System.Web.Http;

    using JetBrains.Annotations;

    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            app.UseWebApi(config);
        }
    }
}
