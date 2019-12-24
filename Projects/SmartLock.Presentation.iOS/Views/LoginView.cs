using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class LoginView : View<ILoginView>, ILoginView
    {
        public event Action LoginClicked;

        public LoginView(LoginController controller) : base(controller, "LoginView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = true;

            BtnLogin.TouchUpInside += (s, e) => LoginClicked?.Invoke();
        }
    }
}

