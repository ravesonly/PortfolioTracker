namespace PortfolioTracker.Data.Services
{
    public interface IStorageService
    {
        int SelectedStock { get; set; }

        int SelectedTrade { get; set; }
    }
}
