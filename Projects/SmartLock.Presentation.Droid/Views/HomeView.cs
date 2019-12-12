using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Controls;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class HomeView : ViewBase<IHomeView>, IHomeView
    {
        private const int StateSearchButton = 0;
        private const int StateLockList = 1;
        private const int StateLock = 2;

        private View _searchingBtnContainer;
        
        private ImageView _ivScanButton;
        private TextView _tvScanButton;
        private RecyclerView _rvBleList;

        private View _lockContainer;
        private TextView _tvLockTitle;
        private TextView _tvLockSubTitle;
        private TextView _tvBatteryStatus;
        private SlideUnlockView _slideUnlockView;

        private bool isScanning;

        private BleDeviceAdapter _adapter;

        protected override int LayoutId => Resource.Layout.View_Home;

        public event Action<bool> StartStop;
        public event Action<BleDevice> Connect;
        public event Action CancelConnect;
        public event Action UnlockClicked;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _searchingBtnContainer = FindViewById<View>(Resource.Id.searchingBtnContainer);
            _ivScanButton = FindViewById<ImageView>(Resource.Id.ivScanButton);
            _tvScanButton = FindViewById<TextView>(Resource.Id.tvScanButton);

            _rvBleList = FindViewById<RecyclerView>(Resource.Id.rvBleList);

            _lockContainer = FindViewById<View>(Resource.Id.lockContainer);
            _tvLockTitle = FindViewById<TextView>(Resource.Id.tvLockTitle);
            _tvLockSubTitle = FindViewById<TextView>(Resource.Id.tvLockSubTitle);
            _tvBatteryStatus = FindViewById<TextView>(Resource.Id.tvBatteryStatus);
            _slideUnlockView = FindViewById<SlideUnlockView>(Resource.Id.SlideUnlockView);

            _ivScanButton.Click += (s, e) =>
            {
                ToggleScanStatus();

                StartStop?.Invoke(isScanning);
            };

            _slideUnlockView.Unlocked += () =>
            {
                UnlockClicked?.Invoke();
            };

            SetMode(StateSearchButton);
        }

        public void Show(List<BleDevice> bleDevices)
        {
            SetMode(StateLockList);

            if (_adapter == null)
            {
                _adapter = new BleDeviceAdapter(bleDevices, Connect, CancelConnect);
                _rvBleList.SetLayoutManager(new LinearLayoutManager(this));
                _rvBleList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.BleDevices = bleDevices;
                _adapter.NotifyDataSetChanged();
            }
        }

        public void Show(BleDevice bleDevice)
        {
            SetMode(StateLock);

            _tvLockTitle.Text = bleDevice.Name;
            _tvBatteryStatus.Text = bleDevice.BatteryLevelString;
        }

        private void SetMode(int state)
        {
            _searchingBtnContainer.Visibility = state == StateSearchButton ? ViewStates.Visible : ViewStates.Gone;
            _rvBleList.Visibility = state == StateLockList ? ViewStates.Visible : ViewStates.Gone;
            _lockContainer.Visibility = state == StateLock ? ViewStates.Visible : ViewStates.Gone;
        }

        private void ToggleScanStatus(bool forceStop = false)
        {
            isScanning = !isScanning && !forceStop;

            _tvScanButton.Text = isScanning ? "Searching" : "Search Lock";

            ConfigureRotatingButton(isScanning);
        }

        private void ConfigureRotatingButton(bool start)
        {
            var anim = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_rotate);
            anim.FillAfter = true;

            if (start)
            {
                _ivScanButton.StartAnimation(anim);
            }
            else
            {
                _ivScanButton.ClearAnimation();
            }
        }
    }
}