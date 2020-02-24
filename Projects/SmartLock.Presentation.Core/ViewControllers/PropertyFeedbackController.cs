using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class PropertyFeedbackController : ViewController<IPropertyFeedbackView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        public bool Mine;

        public Keybox Keybox;
        public Property Property;

        public PropertyFeedbackController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.SubmitClick += View_SubmitClick;
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            if (Mine)
            {
                DoSafeAsync(LoadAllMyFeedback);

                return;
            }

            if (!Keybox.UserId.HasValue) throw new Exception("Invalid keybox.");

            // My keybox
            if (_userSession.UserId == Keybox.UserId.Value)
            {
                DoSafeAsync(LoadData);
            }
            else
            {
                View.Show();
            }
        }

        private async Task LoadAllMyFeedback()
        {
            var feedbacks = await _keyboxService.GetAllPropertyFeedback();

            View.Show(feedbacks);
        }

        private async Task LoadData()
        {
            var feedbacks = await _keyboxService.GetPropertyFeedback(Keybox, Property.PropertyId);

            View.Show(feedbacks);
        }

        private void View_SubmitClick(string feedback)
        {
            if (string.IsNullOrEmpty(feedback))
            {
                _messageBoxService.ShowMessage("Failed", "Cannot send an empty feedback.");
            }

            DoSafeAsync(async () => await SubmitData(feedback));
        }

        private async Task SubmitData(string feedback)
        {
            await _keyboxService.CreatePropertyFeedback(Keybox.KeyboxId, Property.PropertyId, feedback);

            await _messageBoxService.ShowMessageAsync("Success", "Your feedback has been sent successfully.");

            Pop();
        }
    }
}
