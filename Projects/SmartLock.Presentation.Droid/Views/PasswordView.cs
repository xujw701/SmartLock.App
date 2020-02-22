using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PasswordView : ViewBase<IPasswordView>, IPasswordView
    {
        private ImageView _btnBack;

        private EditText _etOldPassword;
        private EditText _etNewPassword1;
        private EditText _etNewPassword2;

        private Button _btnSubmit;

        public event Action BackClick;
        public event Action<string, string, string> SubmitClick;

        protected override int LayoutId => Resource.Layout.View_Password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);

            _etOldPassword = FindViewById<EditText>(Resource.Id.etOldPassword);
            _etNewPassword1 = FindViewById<EditText>(Resource.Id.etNewPassword1);
            _etNewPassword2 = FindViewById<EditText>(Resource.Id.etNewPassword2);

            _btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            _btnBack.Click += (s, e) => BackClick?.Invoke();

            _btnSubmit.Click += (s, e) => SubmitClick?.Invoke(_etOldPassword.Text, _etNewPassword1.Text, _etNewPassword2.Text);
        }
    }
}