using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingView : FragmentView<ISettingView>, ISettingView
    {
        private TextView _tvName;

        private View _btnProfile;
        private View _btnPassword;
        private View _btnFeedback;
        private Button _btnLogout;

        public event Action ProfileClick;
        public event Action PasswordClick;
        public event Action FeedbackClick;
        public event Action LogoutClick;

        protected override int LayoutId => Resource.Layout.View_Setting;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _tvName = _view.FindViewById<TextView>(Resource.Id.tvName);

            _btnProfile = _view.FindViewById<View>(Resource.Id.btnProfile);
            _btnPassword = _view.FindViewById<View>(Resource.Id.btnPassword);
            _btnFeedback = _view.FindViewById<View>(Resource.Id.btnFeedback);
            _btnLogout = _view.FindViewById<Button>(Resource.Id.btnLogout);

            _btnProfile.Click += (s, e) => ProfileClick?.Invoke();
            _btnPassword.Click += (s, e) => PasswordClick?.Invoke();
            _btnFeedback.Click += (s, e) => FeedbackClick?.Invoke();
            _btnLogout.Click += (s, e) => LogoutClick?.Invoke();

            return _view;
        }

        public void Show(string name)
        {
            _tvName.Text = name;
        }
    }
}