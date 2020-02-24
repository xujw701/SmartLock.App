using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxDetailView : ViewBase<IKeyboxDetailView>, IKeyboxDetailView
    {
        private ImageView _btnBack;
        private View _btnLockHistory;
        private View _btnLockEdit;
        private View _btnLockDashboard;
        private View _btnLockData;
        private View _btnFeedback;

        private TextView _tvName;
        private TextView _tvAddress;
        private TextView _tvRoom;
        private TextView _tvToilet;
        private TextView _tvArea;
        private TextView _tvPrice;

        public event Action BackClick;
        public event Action LockHistoryClick;
        public event Action LockEditClick;
        public event Action LockDashboardClick;
        public event Action LockDataClick;
        public event Action FeedbackClick;
        public event Action Refresh;

        protected override int LayoutId => Resource.Layout.View_KeyboxDetail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _btnLockHistory = FindViewById<View>(Resource.Id.btnLockHistory);
            _btnLockEdit = FindViewById<View>(Resource.Id.btnLockEdit);
            _btnLockDashboard = FindViewById<View>(Resource.Id.btnLockDashboard);
            _btnLockData = FindViewById<View>(Resource.Id.btnLockData);
            _btnFeedback = FindViewById<View>(Resource.Id.btnFeedback);

            _tvName = FindViewById<TextView>(Resource.Id.tvName);
            _tvAddress = FindViewById<TextView>(Resource.Id.tvAddress);
            _tvRoom = FindViewById<TextView>(Resource.Id.tvRoom);
            _tvToilet = FindViewById<TextView>(Resource.Id.tvToilet);
            _tvArea = FindViewById<TextView>(Resource.Id.tvArea);
            _tvPrice = FindViewById<TextView>(Resource.Id.tvPrice);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
            _btnLockHistory.Click += (s, e) => LockHistoryClick?.Invoke();
            _btnLockEdit.Click += (s, e) => LockEditClick?.Invoke();
            _btnLockDashboard.Click += (s, e) => LockDashboardClick?.Invoke();
            _btnLockData.Click += (s, e) => LockDataClick?.Invoke();
            _btnFeedback.Click += (s, e) => FeedbackClick?.Invoke();
        }

        protected override void OnResume()
        {
            base.OnResume();

            Refresh?.Invoke();
        }

        public void Show(Keybox keybox, Property property, bool mine)
        {
            _tvName.Text = property.Address;
            _tvAddress.Text = property.PropertyName;
            _tvRoom.Text = property.Bedrooms.HasValue ? property.Bedrooms.Value.ToString() : "N/A";
            _tvToilet.Text = property.Bathrooms.HasValue ? property.Bathrooms.Value.ToString() : "N/A";
            _tvArea.Text = property.FloorAreaString;
            _tvPrice.Text = property.PriceString;

            _btnLockHistory.Visibility = mine ? ViewStates.Visible : ViewStates.Gone;
            _btnLockEdit.Visibility = mine ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}