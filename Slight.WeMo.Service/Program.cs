using System;

namespace Slight.WeMo.Service
{
    using System.Diagnostics;

    using Microsoft.Owin.Hosting;

    using Slight.WeMo.Framework.Discovery;

    static class Program
    {
        static void Main()
        {
            WeMoDiscoverer.Instance.StartSearch();

            var host = Debugger.IsAttached ? "localhost" : "+";
            var baseAddress = $"http://{host}:9000/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Ready.");
                Console.ReadLine();
            }
        }
    }
}
