using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class KeyboxPinView : View<IKeyboxPinView>, IKeyboxPinView
    {
        public event Action BackClick;
        public event Action<string, string, string> SubmitClick;
        public event Action<bool> PinChanged;

        public KeyboxPinView(KeyboxPinController controller) : base(controller, "KeyboxPinView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ConfigureUI();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            BtnSubmit.TouchUpInside += (s, e) => SubmitClick?.Invoke(EtOldPassword.Text, EtNewPassword1.Text, EtNewPassword2.Text);
        }

        private void ConfigureUI()
        {
            var contentWidth = UIScreen.MainScreen.Bounds.Width - 40;

            EtNewPassword1.WidthAnchor.ConstraintEqualTo(contentWidth).Active = true;

            ShadowHelper.AddShadow(EtOldPassword);
            ShadowHelper.AddShadow(EtNewPassword1);
            ShadowHelper.AddShadow(EtNewPassword2);
        }

        public void OnPinChanged(bool success)
        {
            InvokeOnMainThread(() =>
            {
                PinChanged?.Invoke(success);
            });
        }
    }
}