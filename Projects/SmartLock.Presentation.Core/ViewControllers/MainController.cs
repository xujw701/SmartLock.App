using SmartLock.Model.PushNotification;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class MainController : ViewController<IMainView>
    {
        private readonly IUserSession _userSession;
        private readonly IPushNotificationService _pushNotificationService;

        private readonly HomeController _homeController;
        private readonly KeyboxesController _keyboxesController;
        private readonly ListingController _listingController;
        private readonly NearbyController _nearbyController;
        private readonly SettingController _settingController;

        public MainController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IUserService userService, IKeyboxService keyboxService, IPlatformServices platformServices, IPushNotificationService pushNotificationService) : base(viewService)
        {
            _userSession = userSession;
            _pushNotificationService = pushNotificationService;

            _homeController = new HomeController(viewService, messageBoxService, userSession, userService, keyboxService, platformServices);
            _keyboxesController = new KeyboxesController(viewService, messageBoxService, userSession, keyboxService);
            _listingController = new ListingController(viewService);
            _nearbyController = new NearbyController(viewService);
            _settingController = new SettingController(viewService, messageBoxService, userSession, userService);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.SetTabs(_homeController, _keyboxesController, /*_listingController, _nearbyController,*/ _settingController);

            if (_userSession.IsLoggedIn)
            {
                // Register for push notifications
                _pushNotificationService.Register();
            }
        }
    }
}
