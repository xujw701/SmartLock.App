using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class LogsController : ViewController<ILogsView>
    {
        private readonly ITrackedBleService _trackedBleService;

        public LogsController(IViewService viewService, ITrackedBleService trackedBleService) : base(viewService)
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
