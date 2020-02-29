using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class MyProfileController : ViewController<IMyProfileView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IUserService _userService;

        public MyProfileController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IUserService userService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _userService = userService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.SubmitClick += (firstName, lastName, email, phone) => DoSafeAsync(async () => await SubmitData(firstName, lastName, email, phone));
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(_userSession.FirstName, _userSession.LastName, _userSession.Email, _userSession.Phone);
        }

        private async Task SubmitData(string firstName, string lastName, string email, string phone)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
            {
                _messageBoxService.ShowMessage("Failed", "Please input all the fields.");

                return;
            }

            try
            {
                await _userService.UpdateMe(firstName, lastName, email, phone);

                await _messageBoxService.ShowMessageAsync("Success", "Your profile has been updated successfully.");

                Pop();
            }
            catch (Exception )
            {
                await _messageBoxService.ShowMessageAsync("Failed", "Failed to updated profile.");
            }
        }
    }
}
