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
    public class MyProfileView : ViewBase<IMyProfileView>, IMyProfileView
    {
        private ImageView _btnBack;

        private EditText _etFirstName;
        private EditText _etLastName;
        private EditText _etEmail;
        private EditText _etPhone;

        private Button _btnSubmit;

        public event Action BackClick;
        public event Action<string, string, string, string> SubmitClick;

        protected override int LayoutId => Resource.Layout.View_Profile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);

            _etFirstName = FindViewById<EditText>(Resource.Id.etFirstName);
            _etLastName = FindViewById<EditText>(Resource.Id.etLastName);
            _etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            _etPhone = FindViewById<EditText>(Resource.Id.etPhone);

            _btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);


            _btnBack.Click += (s, e) => BackClick?.Invoke();

            _btnSubmit.Click += (s, e) => SubmitClick?.Invoke(_etFirstName.Text.ToString(), _etLastName.Text.ToString(), _etEmail.Text.ToString(), _etPhone.Text.ToString());
        }

        public void Show(string firstName, string lastName, string email, string phone)
        {
            _etFirstName.Text = firstName;
            _etLastName.Text = lastName;
            _etEmail.Text = email;
            _etPhone.Text = phone;
        }
    }
}