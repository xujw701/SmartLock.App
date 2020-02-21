using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxPlaceUpdateController : ViewController<IKeyboxPlaceUpdateView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IKeyboxService _keyboxService;

        public Keybox Keybox;
        public Property Property;

        public KeyboxPlaceUpdateController(IViewService viewService, IMessageBoxService messageBoxService, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.SubmitClick += (property) => DoSafeAsync(SubmitData);

            if (Keybox == null || Property == null)
            {
                throw new Exception("Can not update keybox right now.");
            }
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(Keybox, Property);
        }

        private async Task SubmitData()
        {
            var result = await _keyboxService.PlaceLockUpdate(Keybox, Property);

            if (result)
            {
                await _messageBoxService.ShowMessageAsync("Success", "Your keybox has been updated successfully.");
                Pop();
            }
            else
            {
                await _messageBoxService.ShowMessageAsync("Failed", "Failed to update the keybox.");
            }
        }
    }
}
