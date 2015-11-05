using System;

namespace Slight.WeMo.Service
{
    using System.Diagnostics;

    using Hangfire;

    using Microsoft.Data.Entity;
    using Microsoft.Owin.Hosting;

    using Slight.WeMo.DataAccess;
    using Slight.WeMo.Framework.Actors;

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Running database migrations...");
            using (var db = new WeMoContext())
            {
                db.Database.Migrate();
            }

            var host = Debugger.IsAttached ? "localhost" : "+";
            var baseAddress = $"http://{host}:9000/";

            Console.WriteLine("Starting host...");
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Starting bacground tasks...");
                var discoverer = new WeMoDiscoverer();
                RecurringJob.AddOrUpdate(() => discoverer.Search(), "*/1 * * * *");

                Console.WriteLine("Ready.");
                Console.ReadLine();
            }
        }
    }
}
