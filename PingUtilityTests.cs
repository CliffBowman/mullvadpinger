using System.Net;
using Moq;
using NUnit.Framework;

namespace MullvadPinger.Tests
{
    [TestFixture]
    public class PingUtilityTests
    {
        [Test]
        public void GetAvgPingRateAsyncNullHostnameTest()
        {
            var pingWrapperMock = new Mock<IPingWrapper>();
            var pingerUtil = new PingUtility(pingWrapperMock.Object);
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await pingerUtil.GetAvgPingRateAsync(null));

            Assert.That(ex?.ParamName, Is.EqualTo("hostname"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetAvgPingRateAsyncNegativeTimeoutTest(int value)
        {
            var pingWrapperMock = new Mock<IPingWrapper>();
            var pingerUtil = new PingUtility(pingWrapperMock.Object);
            var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await pingerUtil.GetAvgPingRateAsync("hostname", value));

            Assert.That(ex?.ParamName, Is.EqualTo("timesToPing"));
        }

        [Test]
        public async Task GetAvgPingRateAsyncPingOnceTest()
        {
            var pingWrapperMock = new Mock<IPingWrapper>();
            var pingerUtil = new PingUtility(pingWrapperMock.Object);

            pingWrapperMock
                .Setup(x => x.SendPingAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new PingReplyWrapper
                {
                    Address = IPAddress.Loopback,
                    RoundtripTime = 10,
                });

            var result = await pingerUtil.GetAvgPingRateAsync("somehost", 1);

            Assert.AreEqual(1, pingWrapperMock.Invocations.Count);
            Assert.AreEqual(10, result.MinRate);
            Assert.AreEqual(10, result.AverageRate);
            Assert.AreEqual(10, result.MaxRate);
            Assert.AreEqual(IPAddress.Loopback.ToString(), result.IPAddress);
        }

        [Test]
        public async Task GetAvgPingRateAsyncPingMultipleTest()
        {
            var pingWrapperMock = new Mock<IPingWrapper>();
            var pingerUtil = new PingUtility(pingWrapperMock.Object);

            pingWrapperMock
                .SetupSequence(x => x.SendPingAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new PingReplyWrapper
                {
                    Address = IPAddress.Loopback,
                    RoundtripTime = 10,
                })
                .ReturnsAsync(new PingReplyWrapper
                {
                    Address = IPAddress.Loopback,
                    RoundtripTime = 20,
                })
                .ReturnsAsync(new PingReplyWrapper
                {
                    Address = IPAddress.Loopback,
                    RoundtripTime = 30,
                });

            var result = await pingerUtil.GetAvgPingRateAsync("somehost", 3);

            Assert.AreEqual(3, pingWrapperMock.Invocations.Count);
            Assert.AreEqual(10, result.MinRate);
            Assert.AreEqual(20, result.AverageRate);
            Assert.AreEqual(30, result.MaxRate);
            Assert.AreEqual(IPAddress.Loopback.ToString(), result.IPAddress);
        }
    }
}
