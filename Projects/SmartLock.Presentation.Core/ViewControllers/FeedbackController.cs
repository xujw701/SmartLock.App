using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class FeedbackController : ViewController<IFeedbackView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserService _userService;
        private readonly IKeyboxService _keyboxService;

        public FeedbackController(IViewService viewService, IMessageBoxService messageBoxService, IUserService userService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userService = userService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.SubmitClick += View_SubmitClick;
        }

        private void View_SubmitClick(string feedback)
        {
            if (string.IsNullOrEmpty(feedback))
            {
                _messageBoxService.ShowMessage("Failed", "Cannot send an empty feedback.");

                return;
            }

            DoSafeAsync(async () => await SubmitData(feedback));
        }

        private async Task SubmitData(string feedback)
        {
            await _userService.CreateFeedback(feedback);

            await _messageBoxService.ShowMessageAsync("Success", "Your feedback has been sent successfully.");

            Pop();
        }
    }
}
