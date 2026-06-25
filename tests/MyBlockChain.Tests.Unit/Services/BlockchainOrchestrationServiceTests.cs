using Microsoft.Extensions.Logging;
using Moq;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;

namespace MyBlockChain.Tests.Unit.Services
{
    public class BlockchainOrchestrationServiceTests
    {
        [Fact]
        public async Task FetchAllChainsParallelAsync_Returns_All_Block_Heights()
        {
            var ethService = new Mock<IEthBlockService>();
            var dashService = new Mock<IDashBlockService>();
            var btcService = new Mock<IBtcBlockService>();
            var bitCoinService = new Mock<IBitCoinBlockService>();
            var ltcService = new Mock<ILtcBlockService>();
            var logger = new Mock<ILogger<BlockchainOrchestrationService>>();

            ethService.Setup(s => s.FetchAndSaveAsync()).ReturnsAsync(new EthBlock { Height = 1 });
            dashService.Setup(s => s.FetchAndSaveAsync()).ReturnsAsync(new DashBlock { Height = 2 });
            btcService.Setup(s => s.FetchAndSaveAsync()).ReturnsAsync(new BtcBlock { Height = 3 });
            bitCoinService.Setup(s => s.FetchAndSaveAsync()).ReturnsAsync(new BitCoinBlock { Height = 4 });
            ltcService.Setup(s => s.FetchAndSaveAsync()).ReturnsAsync(new LtcBlock { Height = 5 });

            var service = new BlockchainOrchestrationService(
                ethService.Object,
                dashService.Object,
                btcService.Object,
                bitCoinService.Object,
                ltcService.Object,
                logger.Object);

            var results = await service.FetchAllChainsParallelAsync();

            Assert.Equal(5, results.Count);
            Assert.Equal(1, results["eth"]);
            Assert.Equal(2, results["dash"]);
            Assert.Equal(3, results["btc"]);
            Assert.Equal(4, results["btc-test3"]);
            Assert.Equal(5, results["ltc"]);
        }
    }
}
