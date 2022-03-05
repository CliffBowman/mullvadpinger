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
    }
}
