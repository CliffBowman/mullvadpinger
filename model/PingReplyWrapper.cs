using System.Net;

namespace MullvadPinger.model
{
    public record class PingReplyWrapper
    {
        public IPAddress? Address { get; init; }
        public long RoundtripTime { get; init; }
    }
}
