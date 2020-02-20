using SmartLock.Model.PushNotification;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class LoginController : ViewController<ILoginView>
    {
        private readonly IUserSession _userSession;
        private readonly IUserService _userService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IPushNotificationService _pushNotificationService;

        public LoginController(IViewService viewService, IUserSession userSession, IUserService userService, IMessageBoxService messageBoxService, IPushNotificationService pushNotificationService) : base(viewService)
        {
            _userSession = userSession;
            _userService = userService;
            _messageBoxService = messageBoxService;
            _pushNotificationService = pushNotificationService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            if (_userSession.IsLoggedIn)
            {
                Push<MainController>();
            }

            View.LoginClicked += View_LoginClicked;

            _pushNotificationService.BindDeviceTokenListener();
        }

        private void View_LoginClicked(string username, string password)
        {
            DoSafeAsync(async () =>
                await _userService.Login(username, password),
                successAcction: () =>
                {
                    Push<MainController>();
                });
        }

        protected override async Task ShowErrorAsync(Exception exception)
        {
            await _messageBoxService.ShowMessageAsync("Login Failed", "Please check your username and password.");
        }
    }
}
