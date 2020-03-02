using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Controls;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Linq;

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
        private TextView _tvFeedback;

        private ViewPager _vpMainPager;
        private ImageView _ivPlaceholder;

        private ImagePagerAdapter _imagePagerAdapter;

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
            _tvFeedback = FindViewById<TextView>(Resource.Id.tvFeedback);

            _vpMainPager = FindViewById<ViewPager>(Resource.Id.vp_main_pager);
            _ivPlaceholder = FindViewById<ImageView>(Resource.Id.ivPlaceholder);

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

            _tvFeedback.Text = mine ? "Feedback history" : "Leave a feedback";

            _btnLockHistory.Visibility = mine ? ViewStates.Visible : ViewStates.Gone;
            _btnLockEdit.Visibility = mine ? ViewStates.Visible : ViewStates.Gone;

            SetupViewPager(property);
        }

        private void SetupViewPager(Property property)
        {
            _vpMainPager.OffscreenPageLimit = 3;
            _vpMainPager.PageMargin = 30;

            if (property.PropertyResource != null && property.PropertyResource.Count > 0)
            {
                _vpMainPager.Visibility = ViewStates.Visible;
                _ivPlaceholder.Visibility = ViewStates.Gone;
            }
            else
            {
                _vpMainPager.Visibility = ViewStates.Gone;
                _ivPlaceholder.Visibility = ViewStates.Visible;
                return;
            }

            if (_imagePagerAdapter == null)
            {
                _imagePagerAdapter = new ImagePagerAdapter(this, property.PropertyResource.Select(p => p.Image).ToList(), (c) => { });

                _vpMainPager.Adapter = _imagePagerAdapter;
                _vpMainPager.SetPageTransformer(false, new GalleryPageTransformer());
            }
            else
            {
                _imagePagerAdapter.Items = property.PropertyResource.Select(p => p.Image).ToList();
                _imagePagerAdapter.NotifyDataSetChanged();
            }
        }
    }
}