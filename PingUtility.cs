using System.Net;
using System.Net.NetworkInformation;

namespace MullvadPinger
{
    public class PingUtility
    {
        private readonly IPingWrapper _pingWrapper;

        public PingUtility(IPingWrapper pingWrapper)
        {
            this._pingWrapper = pingWrapper;
        }

        public async Task<PingResult> GetAvgPingRateAsync(string? hostname, int timesToPing = 10)
        {
            if (hostname == null)
                throw new ArgumentNullException(nameof(hostname));

            const int timeout = 1_000;
            List<long> times = new();
            string? ipAddress = null;

            for (var i = 0; i < timesToPing; i++)
            {
                var reply = await _pingWrapper.SendPingAsync(hostname, timeout);

                if (ipAddress == null)
                    ipAddress = reply.Address.ToString();

                times.Add(reply.RoundtripTime);
            }

            return new PingResult
            {
                IPAddress = ipAddress,
                MinRate = times.Min(),
                AverageRate = times.Average(),
                MaxRate = times.Max(),
            };
        }
    }

    public interface IPingWrapper
    {
        Task<PingReplyWrapper> SendPingAsync(string hostname, int timeout);
    }

    public class PingWrapper : IPingWrapper
    {
        public async Task<PingReplyWrapper> SendPingAsync(string hostname, int timeout)
        {
            PingReply? reply;

            using (var ping = new Ping())
                reply = await ping.SendPingAsync(hostname, timeout);

            return new PingReplyWrapper
            {
                Address = reply.Address,
                RoundtripTime = reply.RoundtripTime,
            };
        }
    }

    public class NoopPingWrapper : IPingWrapper
    {
        public Task<PingReplyWrapper> SendPingAsync(string hostname, int timeout)
        {
            return Task.FromResult<PingReplyWrapper>(new PingReplyWrapper
            {
                Address = IPAddress.None,
                RoundtripTime = 0,
            });
        }
    }

    public record class PingResult
    {
        public string? IPAddress { get; init; }
        public long MinRate { get; init; }
        public double AverageRate { get; init; }
        public long MaxRate { get; init; }
    }

    public record class PingReplyWrapper
    {
        public IPAddress Address { get; init; }
        public long RoundtripTime { get; init; }
    }
}
