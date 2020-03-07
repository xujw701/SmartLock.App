using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class MyProfileView : View<IMyProfileView>, IMyProfileView
    {
        public event Action BackClick;
        public event Action<string, string, string, string> SubmitClick;

        public MyProfileView(MyProfileController controller) : base(controller, "MyProfileView")
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

            BtnSubmit.TouchUpInside += (s, e) => SubmitClick?.Invoke(EtFirstName.Text, EtLastName.Text, EtEmail.Text, EtPhone.Text);
        }

        public void Show(string firstName, string lastName, string email, string phone)
        {
            EtFirstName.Text = firstName;
            EtLastName.Text = lastName;
            EtEmail.Text = email;
            EtPhone.Text = phone;
        }

        private void ConfigureUI()
        {
            var contentWidth = UIScreen.MainScreen.Bounds.Width - 40;

            EtFirstName.WidthAnchor.ConstraintEqualTo(contentWidth).Active = true;

            ShadowHelper.AddShadow(EtFirstName);
            ShadowHelper.AddShadow(EtLastName);
            ShadowHelper.AddShadow(EtEmail);
            ShadowHelper.AddShadow(EtPhone);
        }
    }
}

