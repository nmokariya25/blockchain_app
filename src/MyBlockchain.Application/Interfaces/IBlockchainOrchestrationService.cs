namespace MyBlockchain.Application.Interfaces
{
    public interface IBlockchainOrchestrationService
    {
        Task<Dictionary<string, long>> FetchAllChainsParallelAsync();
    }
}
