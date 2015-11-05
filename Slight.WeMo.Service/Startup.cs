using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Slight.WeMo.Service.Startup))]

namespace Slight.WeMo.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.MemoryStorage;

    using JetBrains.Annotations;

    using Swashbuckle.Application;

    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.EnableSwagger("help/{apiVersion}", c =>
            {
                c.IncludeXmlComments("Slight.WeMo.Service.XML");
                c.SingleApiVersion("v1", "Slight.WeMo REST Api");
            }).EnableSwaggerUi("help/ui/{*assetPath}");
            app.UseWebApi(config);

            GlobalConfiguration.Configuration.UseMemoryStorage();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = new IAuthorizationFilter[] { new AllowAllFilter() }
            });
            app.UseHangfireServer();

            app.Use(async (context, func) =>
            {
                if (new[] { "/", "/help" }.Any(x => context.Request.Path.Value.StartsWith(x)))
                {
                    context.Response.Redirect("/help/ui/index");
                    return;
                }
                await func.Invoke();
            });
        }
    }

    public class AllowAllFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            return true;
        }
    }
}
