using System.Net;
using System.Net.NetworkInformation;
using MullvadPinger.model;

namespace MullvadPinger
{
    public class PingWrapper : IPingWrapper
    {
        public async Task<PingReplyWrapper> SendPingAsync(string hostname, int timeout)
        {
            PingReply? reply;
            using Ping ping = new();

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
}
