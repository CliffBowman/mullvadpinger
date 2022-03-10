using MullvadPinger.model;

namespace MullvadPinger
{
    public interface IPingWrapper
    {
        Task<PingReplyWrapper> SendPingAsync(string hostname, int timeout);
    }
}
