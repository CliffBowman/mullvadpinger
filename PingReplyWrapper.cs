using System.Net;

namespace MullvadPinger
{
    public record class PingReplyWrapper
    {
        public IPAddress? Address { get; init; }
        public long RoundtripTime { get; init; }
    }
}
