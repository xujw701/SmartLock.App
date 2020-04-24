using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutView : ViewBase<IAboutView>, IAboutView
    {
        private ImageView _btnBack;
        private TextView _tvAppVersion;
        private View _btnFeedback;

        protected override int LayoutId => Resource.Layout.View_About;

        public event Action BackClick;
        public event Action FeedbackClick;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _tvAppVersion = FindViewById<TextView>(Resource.Id.tvAppVersion);
            _btnFeedback = FindViewById<View>(Resource.Id.btnFeedback);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
            _btnFeedback.Click += (s, e) => FeedbackClick?.Invoke();

            _tvAppVersion.Text = "App Version: " + Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
        }
    }
}