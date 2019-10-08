namespace PortfolioTracker.Data.Services
{
    public class StorageService : IStorageService
    {
        public int SelectedStock { get; set; }
        public int SelectedTrade { get; set; }
    }
}
