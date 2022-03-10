using MullvadPinger.model;

namespace MullvadPinger
{
    public interface IPingUtility
    {
        Task<PingResult> GetAvgPingRateAsync(string? hostname, int timesToPing = 5, int pingIntervalMS = 200);
    }
}
