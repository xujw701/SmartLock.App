using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxDetailView : ViewBase<IKeyboxDetailView>, IKeyboxDetailView
    {
        private ImageView _btnBack;
        private View _btnLockHistory;
        private View _btnLockDashboard;

        public event Action BackClick;
        public event Action LockHistoryClick;
        public event Action LockDashboardClick;

        protected override int LayoutId => Resource.Layout.View_KeyboxDetail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _btnLockHistory = FindViewById<View>(Resource.Id.btnLockHistory);
            _btnLockDashboard = FindViewById<View>(Resource.Id.btnLockDashboard);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
            _btnLockHistory.Click += (s, e) => LockHistoryClick?.Invoke();
            _btnLockDashboard.Click += (s, e) => LockDashboardClick?.Invoke();
        }

        public void Show(Keybox keybox)
        {
        }
    }
}