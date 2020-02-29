using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxPlaceController : ViewController<IKeyboxPlaceView>
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IUserSession _userSession;
        private readonly IKeyboxService _keyboxService;

        public Keybox Keybox;
        private Property _property;

        private Keybox ConnectedKeybox => Keybox ?? throw new Exception("Please connect to a keybox first.");
        private bool IsListed => ConnectedKeybox != null && ConnectedKeybox.PropertyId.HasValue;

        public KeyboxPlaceController(IViewService viewService, IMessageBoxService messageBoxService, IUserSession userSession, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _userSession = userSession;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.AttachmentAdded += View_AttachmentAdded;
            View.AttachmentDeleted += View_AttachmentDeleted;
            View.SubmitClick += (property) => DoSafeAsync(SubmitData);

            if (_property == null)
            {
                _property = new Property();
            }
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(ConnectedKeybox, _property);

            if (IsListed)
            {
                _messageBoxService.ShowMessage("Please notice", "This keybox has been placed already. Place this lock will end your last listing.");
            }
        }

        private void View_AttachmentAdded(byte[] data)
        {
            var photos = _property.PropertyResource.Where(pr => !pr.ToDelete).Select(pr => pr.Image).Union(_property.ToUploadResource);

            if (photos.Count() > 3)
            {
                View.Show(Keybox, _property);

                _messageBoxService.ShowMessage("Tips", "You can only upload 4 photos.");

                return;
            }

            // Save to local
            var attachment = _keyboxService.SavePropertyResourceLocal(data);
            _property.ToUploadResource.Add(attachment);

            View.Show(ConnectedKeybox, _property);
        }

        private void View_AttachmentDeleted(Cache cache)
        {
            // Check if it is local attachment
            if (_property.ToUploadResource.Contains(cache))
            {
                _property.ToUploadResource.Remove(cache);
            }
            else
            {
                var resouce = _property.PropertyResource.FirstOrDefault(pr => pr.Image == cache);

                if (resouce == null) throw new Exception("Internal error, the resource is invalid."); // Shouldn't go here

                var index = _property.PropertyResource.IndexOf(resouce);
                _property.PropertyResource[index].ToDelete = true;
            }

            View.Show(ConnectedKeybox, _property);
        }

        private async Task SubmitData()
        {
            if (IsListed)
            {
                await _keyboxService.EndKeyboxProperty(ConnectedKeybox.KeyboxId, ConnectedKeybox.PropertyId.Value);
            }

            var result = await _keyboxService.PlaceLock(ConnectedKeybox, _property);

            if (result)
            {
                await _messageBoxService.ShowMessageAsync("Success", "Your keybox has been placed successfully.");
                Pop();
            }
            else
            {
                await _messageBoxService.ShowMessageAsync("Failed", "Failed to place the keybox.");
            }
        }
    }
}
