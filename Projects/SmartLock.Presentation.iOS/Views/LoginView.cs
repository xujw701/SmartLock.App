using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class LoginView : View<ILoginView>, ILoginView
    {
        public event Action<string, string> LoginClicked;
        public event Action<bool> RememberMeClicked;
        public event Action EnvironmentSettingClicked;

        private bool _rememberMe;

        public LoginView(LoginController controller) : base(controller, "LoginView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvBackground.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {

                EtUsername.ResignFirstResponder();
                EtPassword.ResignFirstResponder();
            }));

            var clickCnt = 0;
            IvLogo.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                if (clickCnt == 10)
                {
                    clickCnt = 0;
                    EnvironmentSettingClicked?.Invoke();
                }
                else
                {
                    clickCnt++;
                }
            }));
            IvLogo.Layer.CornerRadius = 12;
            IvLogo.Layer.MasksToBounds = true;
            
            ShadowHelper.AddShadow(IvLogo);

#if DEBUG
            EtUsername.Text = "william";
            EtPassword.Text = "123";
#endif

            BtnLogin.TouchUpInside += (s, e) => LoginClicked?.Invoke(EtUsername.Text, EtPassword.Text);
            IvCheckbox.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                UpdateRememberMe(!_rememberMe);

                RememberMeClicked?.Invoke(_rememberMe);
            }));
        }

        public void Show(bool rememberMe)
        {
            UpdateRememberMe(rememberMe);
        }

        private void UpdateRememberMe(bool rememberMe)
        {
            _rememberMe = rememberMe;

            IvCheckbox.Image = _rememberMe ? UIImage.FromBundle("icon_checked") : UIImage.FromBundle("icon_unchecked");
        
        }
    }
}