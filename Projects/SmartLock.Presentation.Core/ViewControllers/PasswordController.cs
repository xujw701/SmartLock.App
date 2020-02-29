using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class PasswordController : ViewController<IPasswordView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IUserService _userService;

        public PasswordController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IUserService userService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _userService = userService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.SubmitClick += (old, new1, new2) => DoSafeAsync(async () => await SubmitData(old, new1, new2));
        }

        private async Task SubmitData(string old, string new1, string new2)
        {
            if (string.IsNullOrEmpty(old) || string.IsNullOrEmpty(new1) || string.IsNullOrEmpty(new2))
            {
                _messageBoxService.ShowMessage("Failed", "Please input all the fields.");

                return;
            }
            if (!new1.Equals(new2))
            {
                _messageBoxService.ShowMessage("Failed", "New password doesn't match, please check it again.");

                return;
            }

            try
            {
                var result = await _userService.UpdatePassword(old, new2);

                if (result)
                {
                    await _messageBoxService.ShowMessageAsync("Success", "Your profile has been updated successfully.");

                    Pop();
                }
                else
                {
                    await _messageBoxService.ShowMessageAsync("Failed", "Failed to change password.");
                }
            }
            catch (Exception )
            {
                await _messageBoxService.ShowMessageAsync("Failed", "Failed to change password.");

                return;
            }
        }
    }
}
