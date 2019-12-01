using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class MainController : ViewController<IMainView>
    {
        private readonly HomeController _homeViewController;
        private readonly MyLockController _myLockViewController;
        private readonly LogsController _logsViewController;
        private readonly SettingController _settingViewController;

        public MainController(IViewService viewService) : base(viewService)
        {
            _homeViewController = new HomeController(viewService);
            _myLockViewController = new MyLockController(viewService);
            _logsViewController = new LogsController(viewService);
            _settingViewController = new SettingController(viewService);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.SetTabs(_homeViewController, _myLockViewController, _logsViewController, _settingViewController);
        }
    }
}
