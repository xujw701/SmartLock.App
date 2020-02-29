using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Linq;
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
            View.AttachmentAdded += View_AttachmentAdded;
            View.AttachmentDeleted += View_AttachmentDeleted;
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

        private void View_AttachmentAdded(byte[] data)
        {
            // Save to local
            var attachment = _keyboxService.SavePropertyResourceLocal(data);
            Property.ToUploadResource.Add(attachment);

            View.Show(Keybox, Property);
        }

        private void View_AttachmentDeleted(Cache cache)
        {
            // Check if it is local attachment
            if (Property.ToUploadResource.Contains(cache))
            {
                Property.ToUploadResource.Remove(cache);
            }
            else
            {
                var resouce = Property.PropertyResource.FirstOrDefault(pr => pr.Image == cache);

                if (resouce == null) throw new Exception("Internal error, the resource is invalid."); // Shouldn't go here

                var index = Property.PropertyResource.IndexOf(resouce);
                Property.PropertyResource[index].ToDelete = true;
            }

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
