using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using MullvadPinger.model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;

namespace MullvadPinger
{
    [TestFixture]
    public class MullvadClientTests
    {
        [Test]
        public void ValidateSchemaTest()
        {
            var schema = JSchema.Parse(File.ReadAllText(@"schemas/mullvad-www-relays-all-response-schema.json"));
            var serverList = JArray.Parse(File.ReadAllText(@"data/mullvad-www-relays-all-response-sample.json"));

            IList<string> errors = new List<string>();

            var isValid = serverList.IsValid(schema, out errors);

            errors.ToList().ForEach(Console.WriteLine);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void FrameworkJsonTest()
        {
            // var result = JsonSerializer.Deserialize<List<MullvadVPNServer>>(File.ReadAllText(@"data/mullvad-www-relays-all-response-sample.json"));
            var mockLogger = new Mock<ILogger<MullvadClient>>();

            // mockLogger
            //     .Setup(l => l.LogError(It.IsAny<JsonException>(), It.IsAny<string>())
            //     .

            var result = new MullvadClient(new SampleMullvadDataSource(), mockLogger.Object).GetVPNServerListAsync().GetAwaiter().GetResult();

        }

        [Test]
        public async Task GetVPNServerListAsyncTest()
        {
            var mockDataSource = new Mock<IMullvadDataSource>();
            var mockLogger = new Mock<ILogger<MullvadClient>>();
            var mullvadClient = new MullvadClient(mockDataSource.Object, mockLogger.Object);

            mockDataSource
                .Setup(x => x.GetVPNServerJsonAsync())
                .ReturnsAsync(@"
                [
                    {
                            ""hostname"": ""us209-wireguard"",
                            ""country_code"": ""us"",
                            ""country_name"": ""USA"",
                            ""city_code"": ""sea"",
                            ""city_name"": ""Seattle, WA"",
                            ""active"": true,
                            ""owned"": false,
                            ""provider"": ""100TB"",
                            ""ipv4_addr_in"": ""199.229.250.52"",
                            ""ipv6_addr_in"": ""2607:f7a0:c:4::c09f"",
                            ""network_port_speed"": 1,
                            ""type"": ""wireguard"",
                            ""status_messages"": [],
                            ""pubkey"": ""APxS9ebzK537njzcfB9gh8VXWrFrKvZeC6QQe0ZCUUM="",
                            ""multihop_port"": 3098,
                            ""socks_name"": ""us209-wg.socks5""
                        },
                        {
                            ""hostname"": ""us210-wireguard"",
                            ""country_code"": ""us"",
                            ""country_name"": ""USA"",
                            ""city_code"": ""sea"",
                            ""city_name"": ""Seattle, WA"",
                            ""active"": true,
                            ""owned"": false,
                            ""provider"": ""100TB"",
                            ""ipv4_addr_in"": ""199.229.250.53"",
                            ""ipv6_addr_in"": ""2607:f7a0:c:4::c10f"",
                            ""network_port_speed"": 1,
                            ""type"": ""wireguard"",
                            ""status_messages"": [],
                            ""pubkey"": ""92KRwUmhQY/n5cAUKR1R/Z/z17wOmB08GZxuats8cEw="",
                            ""multihop_port"": 3101,
                            ""socks_name"": ""us210-wg.socks5""
                        },
                ]
                ");

            var resultVPNServerList = await mullvadClient.GetVPNServerListAsync();

            Assert.AreEqual(1, mockDataSource.Invocations.Count);
            Assert.AreEqual(2, resultVPNServerList.Count);

            var us209 = resultVPNServerList.SingleOrDefault(x => x.Hostname?.StartsWith("us209") == true);

            Assert.NotNull(us209);
            Assert.AreEqual("us209-wireguard.mullvad.net", us209?.Hostname);
            Assert.AreEqual("APxS9ebzK537njzcfB9gh8VXWrFrKvZeC6QQe0ZCUUM=", us209?.PublicKey);
            Assert.AreEqual(1, us209?.SpeedInGbps);

            var us210 = resultVPNServerList.SingleOrDefault(x => x.Hostname?.StartsWith("us210") == true);

            Assert.NotNull(us210);
            Assert.AreEqual("us210-wireguard.mullvad.net", us210?.Hostname);
            Assert.AreEqual("92KRwUmhQY/n5cAUKR1R/Z/z17wOmB08GZxuats8cEw=", us210?.PublicKey);
            Assert.AreEqual(1, us210?.SpeedInGbps);
        }

        [Test]
        public void GetVPNServerListAsyncFilterTest()
        {
            var mockDataSource = new Mock<IMullvadDataSource>();
            var mockLogger = new Mock<ILogger<MullvadClient>>();
            var mullvadClient = new MullvadClient(mockDataSource.Object, mockLogger.Object);

            mockDataSource
                .Setup(x => x.GetVPNServerJsonAsync())
                .ReturnsAsync(@"
                [
                    {
                            ""hostname"": ""us209-wireguard"",
                            ""country_code"": ""us"",
                            ""country_name"": ""USA"",
                            ""city_code"": ""sea"",
                            ""city_name"": ""Seattle, WA"",
                            ""active"": true,
                            ""owned"": false,
                            ""provider"": ""100TB"",
                            ""ipv4_addr_in"": ""199.229.250.52"",
                            ""ipv6_addr_in"": ""2607:f7a0:c:4::c09f"",
                            ""network_port_speed"": 1,
                            ""type"": ""wireguard"",
                            ""status_messages"": [],
                            ""pubkey"": ""APxS9ebzK537njzcfB9gh8VXWrFrKvZeC6QQe0ZCUUM="",
                            ""multihop_port"": 3098,
                            ""socks_name"": ""us209-wg.socks5""
                        },
                        {
                            ""hostname"": ""us210-wireguard"",
                            ""country_code"": ""us"",
                            ""country_name"": ""USA"",
                            ""city_code"": ""sea"",
                            ""city_name"": ""Seattle, WA"",
                            ""active"": true,
                            ""owned"": false,
                            ""provider"": ""100TB"",
                            ""ipv4_addr_in"": ""199.229.250.53"",
                            ""ipv6_addr_in"": ""2607:f7a0:c:4::c10f"",
                            ""network_port_speed"": 1,
                            ""type"": ""wireguard"",
                            ""status_messages"": [],
                            ""pubkey"": ""92KRwUmhQY/n5cAUKR1R/Z/z17wOmB08GZxuats8cEw="",
                            ""multihop_port"": 3101,
                            ""socks_name"": ""us210-wg.socks5""
                        }
                ]
                ");

            var resultVPNServerList = mullvadClient.GetVPNServerListAsync(filter: new VPNServerListFilter
            {
                Hostname = "us209",
            }).GetAwaiter().GetResult();

            Assert.AreEqual(1, resultVPNServerList.Count);
            Assert.AreEqual("us209-wireguard.mullvad.net", resultVPNServerList[0].Hostname);
        }
    }
}
