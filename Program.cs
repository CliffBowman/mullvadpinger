using System;
using CommandLine;
using Microsoft.Practices.Unity;

namespace MullvadPinger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IMullvadClient, MullvadClient>();
            // container.RegisterType<IMullvadDataSource, MullvadDataSource>();
            container.RegisterType<IMullvadDataSource, SampleMullvadDataSource>();
            // container.RegisterType<IPingWrapper, PingWrapper>();
            container.RegisterType<IPingWrapper, NoopPingWrapper>();

            Parser.Default
                .ParseArguments<CommandLineArgs>(args)
                .WithParsed<CommandLineArgs>(options =>
                {
                    var mullvadClient = container.Resolve<IMullvadClient>();
                    var servers = mullvadClient.GetVPNServerListAsync().GetAwaiter().GetResult();

                    servers.ForEach(Console.WriteLine);
                });
        }
    }
}
