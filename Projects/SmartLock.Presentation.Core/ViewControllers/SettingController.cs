using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class SettingController : ViewController<ISettingView>
    {
        private readonly IUserSession _userSession;
        private readonly IMessageBoxService _messageBoxService;

        public SettingController(IViewService viewService, IUserSession userSession, IMessageBoxService messageBoxService) : base(viewService)
        {
            _userSession = userSession;
            _messageBoxService = messageBoxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.ProfileClick += () => { };
            View.PasswordClick += () => { };
            View.FeedbackClick += () => { };
            View.LogoutClick += async () =>
            {
                var result = await _messageBoxService.ShowQuestion("Log out", "Are you sure to log out?", "Yes", "Cancel");

                if (result == MessageBoxButtons.Button1)
                {
                    _userSession.LogOut();
                    PopToRoot();
                }
            };
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show($"{_userSession.FirstName} {_userSession.LastName}");
        }
    }
}
