using Microsoft.Extensions.Logging;
using MyBlockchain.Application.Interfaces;

namespace MyBlockchain.Application.Services
{
    public class BlockchainOrchestrationService : IBlockchainOrchestrationService
    {
        private readonly IEthBlockService _ethBlockService;
        private readonly IDashBlockService _dashBlockService;
        private readonly IBtcBlockService _btcBlockService;
        private readonly IBitCoinBlockService _bitCoinBlockService;
        private readonly ILtcBlockService _ltcBlockService;
        private readonly ILogger<BlockchainOrchestrationService> _logger;

        public BlockchainOrchestrationService(
            IEthBlockService ethBlockService,
            IDashBlockService dashBlockService,
            IBtcBlockService btcBlockService,
            IBitCoinBlockService bitCoinBlockService,
            ILtcBlockService ltcBlockService,
            ILogger<BlockchainOrchestrationService> logger)
        {
            _ethBlockService = ethBlockService;
            _dashBlockService = dashBlockService;
            _btcBlockService = btcBlockService;
            _bitCoinBlockService = bitCoinBlockService;
            _ltcBlockService = ltcBlockService;
            _logger = logger;
        }

        public async Task<Dictionary<string, long>> FetchAllChainsParallelAsync()
        {
            var ethTask = _ethBlockService.FetchAndSaveAsync();
            var dashTask = _dashBlockService.FetchAndSaveAsync();
            var btcTask = _btcBlockService.FetchAndSaveAsync();
            var btcTest3Task = _bitCoinBlockService.FetchAndSaveAsync();
            var ltcTask = _ltcBlockService.FetchAndSaveAsync();

            try
            {
                await Task.WhenAll(ethTask, dashTask, btcTask, btcTest3Task, ltcTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "One or more blockchain fetch operations failed during parallel execution.");
                throw;
            }

            return new Dictionary<string, long>
            {
                ["eth"] = (await ethTask).Height,
                ["dash"] = (await dashTask).Height,
                ["btc"] = (await btcTask).Height,
                ["btc-test3"] = (await btcTest3Task).Height,
                ["ltc"] = (await ltcTask).Height
            };
        }
    }
}
