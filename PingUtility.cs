using MullvadPinger.model;

namespace MullvadPinger
{
    public class PingUtility : IPingUtility
    {
        private readonly int DefaultPingTimeout = 1_000;
        private readonly IPingWrapper _pingWrapper;

        public PingUtility(IPingWrapper pingWrapper)
        {
            this._pingWrapper = pingWrapper;
        }

        public async Task<PingResult> GetAvgPingRateAsync(string? hostname, int timesToPing = 5)
        {
            if (hostname == null)
                throw new ArgumentNullException(nameof(hostname));

            if (timesToPing < 1)
                throw new ArgumentOutOfRangeException(nameof(timesToPing));

            List<long> times = new();
            string? ipAddress = null;

            for (var i = 0; i < timesToPing; i++)
            {
                var reply = await _pingWrapper.SendPingAsync(hostname, DefaultPingTimeout);

                if (ipAddress == null)
                    ipAddress = reply.Address?.ToString();

                times.Add(reply.RoundtripTime);
            }

            return new PingResult
            {
                Hostname = hostname,
                IPAddress = ipAddress,
                MinRate = times.Min(),
                AverageRate = times.Average(),
                MaxRate = times.Max(),
            };
        }
    }
}
