using System;

namespace Slight.WeMo.Service
{
    using Microsoft.Owin.Hosting;

    using Slight.WeMo.Framework.Discovery;

    static class Program
    {
        static void Main()
        {
            WeMoDiscoverer.Instance.StartSearch();

            const string baseAddress = "http://+:9000/";
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Ready.");
                Console.ReadLine();
            }
        }
    }
}
