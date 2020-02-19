using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginView : ViewBase<ILoginView>, ILoginView
    {
        protected override int LayoutId => Resource.Layout.View_Login;

        public event Action<string, string> LoginClicked;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            var etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            var etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            btnLogin.Click += (s, e) => { LoginClicked?.Invoke(etUsername.Text, etPassword.Text); };
        }
    }
}