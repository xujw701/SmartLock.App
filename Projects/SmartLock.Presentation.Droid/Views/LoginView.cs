using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginView : ViewBase<ILoginView>, ILoginView
    {
        protected override int LayoutId => Resource.Layout.View_Login;

        public event Action<string, string> LoginClicked;
        public event Action<bool> RememberMeClicked;
        public event Action EnvironmentSettingClicked;

        private CheckBox _cbRememberMe;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var logo = FindViewById<ImageView>(Resource.Id.logo);
            var btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            var etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            var etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            _cbRememberMe = FindViewById<CheckBox>(Resource.Id.cbRememberMe);

            var clickCnt = 0;
            logo.Click += (s, e) =>
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
            };

#if DEBUG
            etUsername.Text = "william";
            etPassword.Text = "123";
#endif

            btnLogin.Click += (s, e) =>
            {
                LoginClicked?.Invoke(etUsername.Text, etPassword.Text);
            };

            _cbRememberMe.Click += (s, e) =>
            {
                RememberMeClicked?.Invoke(_cbRememberMe.Checked);
            };
        }

        public void Show(bool rememberMe)
        {
            _cbRememberMe.Checked = rememberMe;
        }
    }
}