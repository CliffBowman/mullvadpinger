using CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MullvadPinger.model;
using System.Collections.Concurrent;

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
                    services.AddTransient<IPingUtility, PingUtility>();
                })
                .Build();

            Parser.Default
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(options =>
                {
                    var mullvadClient = host.Services.GetRequiredService<IMullvadClient>();
                    var pingUtility = host.Services.GetRequiredService<IPingUtility>();
                    var logger = host.Services.GetService<ILogger<Program>>();
                    var serversToPing = mullvadClient.GetVPNServerListAsync(filter: options).GetAwaiter().GetResult();

                    ConcurrentBag<PingResult> pingResults = new();

                    logger.LogInformation("Starting parallel server ping.");

                    Parallel.ForEach(serversToPing, new ParallelOptions { MaxDegreeOfParallelism = options.NumbersServersToPingInParallel }, async server =>
                    {
                        var pingResult = await pingUtility.GetAvgPingRateAsync(server.FullyQualifiedHostname());

                        pingResults.Add(pingResult);
                    });

                    var sortedResults = pingResults
                        .Join(serversToPing, pr => pr.Hostname, s => s.FullyQualifiedHostname(), (pr, s) => new
                        {
                            Server = s,
                            PingResult = pr,
                        })
                        .OrderBy(r => r.PingResult.AverageRate)
                        .ThenByDescending(r => r.Server.SpeedInGbps)
                        .ToList();

                    sortedResults.ForEach(r =>
                    {
                        Console.WriteLine($"{r.Server.FullyQualifiedHostname()} - {r.PingResult.AverageRate} ms");
                    });
                });
        }
    }
}
