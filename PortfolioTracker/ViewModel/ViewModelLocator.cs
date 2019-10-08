using PortfolioTracker.BL;
using PortfolioTracker.Data.Services;
using PortfolioTracker.ViewModels;
using Unity;

namespace PortfolioTracker
{
    public class ViewModelLocator
    {
        private UnityContainer _container;

        public ViewModelLocator()
        {
            _container = new UnityContainer();

            _container.RegisterType<MainWindowViewModel>();
            _container.RegisterType<ITrackerBusinessLogic, TrackerBusinessLogic>();
            _container.RegisterType<TrackerBusinessLogic>();
            _container.RegisterType<IDatabaseService, DatabaseService>();
            _container.RegisterSingleton<IStorageService, StorageService>();
        }

        public MainWindowViewModel MainWindowViewModel => _container.Resolve<MainWindowViewModel>();
        public TrackerBusinessLogic TrackerBusinessLogic => _container.Resolve<TrackerBusinessLogic>();



    }
}