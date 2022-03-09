using CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MullvadPinger.model;

namespace MullvadPinger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging(logging =>
                    {
                        logging.AddConsole();
                    });

                    services.AddTransient<IMullvadClient, MullvadClient>();
                    // services.AddTransient<IMullvadDataSource, MullvadDataSource>();
                    services.AddTransient<IMullvadDataSource, SampleMullvadDataSource>();
                    // services.AddTransient<IPingWrapper, PingWrapper>();
                    services.AddTransient<IPingWrapper, NoopPingWrapper>();
                })
                .Build();

            Parser.Default
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(options =>
                {
                    var mullvadClient = host.Services.GetRequiredService<IMullvadClient>();
                    var servers = mullvadClient.GetVPNServerListAsync().GetAwaiter().GetResult();

                    servers.ForEach(s => Console.WriteLine($"{s.FullyQualifiedHostname()}"));
                });
        }
    }
}
