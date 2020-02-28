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
        private readonly IUserService _userService;

        public SettingController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IUserService userService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _userService = userService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.PortraitChanged += (data) => DoSafeAsync(async () => await UpdatePortrait(data));
            View.ProfileClick += () => { Push<MyProfileController>(); };
            View.PasswordClick += () => { Push<PasswordController>(); };
            View.FeedbackClick += () => { Push<FeedbackController>(); };
            View.LogoutClick += async () =>
            {
                var result = await _messageBoxService.ShowQuestion("Log out", "Are you sure to log out?", "Cancel", "OK");

                if (result == MessageBoxButtons.Button2)
                {
                    DoSafeAsync(LogOut);
                }
            };

            View.Refresh += () => DoSafeAsync(async () => await LoadData());
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            DoSafeAsync(async () => await LoadData(true));
        }

        private async Task LoadData(bool force = false)
        {
            var portrait = await _userService.GetCachedMyPortrait(force);

            View.Show($"{_userSession.FirstName} {_userSession.LastName}", portrait);
        }

        private async Task UpdatePortrait(byte[] data)
        {
            await _userService.UpdatePortrait(data);

            await _userService.GetCachedMyPortrait(true);

            await LoadData();
        }

        private async Task LogOut()
        {
            await _userService.LogOut();

            PopToRoot();
        }
    }
}
