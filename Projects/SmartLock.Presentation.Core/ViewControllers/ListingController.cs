using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class ListingController : ViewController<IListingView>
    {
        private readonly ITrackedBleService _trackedBleService;

        public ListingController(IViewService viewService, ITrackedBleService trackedBleService) : base(viewService)
        {
            _trackedBleService = trackedBleService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            _trackedBleService.Init();

            View.Title = "Logs";
            View.Show(_trackedBleService.Records);
        }
    }
}