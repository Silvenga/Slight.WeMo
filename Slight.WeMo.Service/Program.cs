using System;

namespace Slight.WeMo.Service
{
    using System.Diagnostics;

    using Hangfire;

    using Microsoft.Data.Entity;
    using Microsoft.Owin.Hosting;

    using Slight.WeMo.DataAccess;
    using Slight.WeMo.Framework.Actors;
    using Slight.WeMo.Framework.Tasks;

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
                Console.WriteLine("Starting background tasks...");
                StartBackgroundTasks();

                Console.WriteLine("Ready.");
                Console.ReadLine();
            }
        }

        private static void StartBackgroundTasks()
        {
            var discoverer = new DiscovererTask();
            RecurringJob.AddOrUpdate(() => discoverer.Search(), Cron.Minutely());

            var recorder = new RecordStatesTask();
            RecurringJob.AddOrUpdate(() => recorder.RecordState(), Cron.Minutely());
        }
    }
}
