using SmartLock.Model.PushNotification;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class SettingController : ViewController<ISettingView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IPushNotificationService _pushNotificationService;

        public SettingController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IPushNotificationService pushNotificationService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _pushNotificationService = pushNotificationService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.ProfileClick += () => { Push<MyProfileController>(); };
            View.PasswordClick += () => { Push<PasswordController>(); };
            View.FeedbackClick += () => { };
            View.LogoutClick += async () =>
            {
                var result = await _messageBoxService.ShowQuestion("Log out", "Are you sure to log out?", "Cancel", "OK");

                if (result == MessageBoxButtons.Button2)
                {
                    DoSafeAsync(LogOut);
                }
            };

            View.Refresh += async () =>
            {
                View.Show($"{_userSession.FirstName} {_userSession.LastName}");
            };
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show($"{_userSession.FirstName} {_userSession.LastName}");
        }

        private async Task LogOut()
        {
            await _pushNotificationService.UnregisterAsync();

            _userSession.LogOut();

            PopToRoot();
        }
    }
}
