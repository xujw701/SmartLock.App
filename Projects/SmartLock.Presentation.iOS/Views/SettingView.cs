using System;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class SettingView : View<ISettingView>, ISettingView
    {
        protected override bool CanSwipeBack => false;

        public event Action<byte[]> PortraitChanged;
        public event Action ProfileClick;
        public event Action PasswordClick;
        public event Action AboutClick;
        public event Action LogoutClick;
        public event Action Refresh;

        public SettingView(SettingController controller) : base(controller, "SettingView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvPortrait.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                ChooseCaptureMethod();
            }));

            BtnProfile.TouchUpInside += (s, e) => ProfileClick?.Invoke();
            BtnPassword.TouchUpInside += (s, e) => PasswordClick?.Invoke();
            BtnAbout.TouchUpInside += (s, e) => AboutClick?.Invoke();
            BtnLogout.TouchUpInside += (s, e) => LogoutClick?.Invoke();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Refresh?.Invoke();
        }

        public void Show(string name, Cache portrait)
        {
            LblName.Text = name;
            ImageHelper.SetImageView(IvPortrait, portrait);
        }

        #region Caches

        [Export("imagePickerController:didFinishPickingMediaWithInfo:")]
        public void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            var isImage = false;

            switch (info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    isImage = true;
                    break;
                case "public.video":
                    Console.WriteLine("Video selected");
                    break;
            }

            if (isImage)
            {
                // get the original image
                var originalImage = info[UIImagePickerController.OriginalImage] as UIImage;
                if (originalImage != null)
                {
                    using (NSData imageData = originalImage.AsJPEG(0.8f)) // TODO: Quality settings? 
                    {
                        var myByteArray = new byte[imageData.Length];

                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));

                        PortraitChanged?.Invoke(myByteArray);
                    }
                }
                else
                {
                    throw new Exception("Image is blank");
                }
            }

            picker.DismissModalViewController(true);

            AppDelegate.SetWhiteStatusBar();
        }

        [Export("imagePickerControllerDidCancel:")]
        public void Canceled(UIImagePickerController picker)
        {
            picker.DismissModalViewController(true);
            AppDelegate.SetWhiteStatusBar();
        }

        private void ChooseCaptureMethod()
        {
            var actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
            actionSheetAlert.AddAction(UIAlertAction.Create("Take a photo", UIAlertActionStyle.Default, (action) =>
            {
                CreatePicker(true);
            }));
            actionSheetAlert.AddAction(UIAlertAction.Create("Add existing photo", UIAlertActionStyle.Default, (action) =>
            {
                CreatePicker(false);
            }));
            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => { }));

            var presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            PresentViewController(actionSheetAlert, true, null);
        }

        private void CreatePicker(bool takePhoto)
        {
            // Show the native image picker controller
            var imagePicker = new UIImagePickerController
            {
                Delegate = this,
                SourceType =
                    takePhoto
                        ? UIImagePickerControllerSourceType.Camera
                        : UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(takePhoto ? UIImagePickerControllerSourceType.Camera : UIImagePickerControllerSourceType.PhotoLibrary)
            };
            AppDelegate.SetBlackStatusBar();
            NavigationController.PresentViewController(imagePicker, true, AppDelegate.SetBlackStatusBar);
        }

        #endregion
    }
}

