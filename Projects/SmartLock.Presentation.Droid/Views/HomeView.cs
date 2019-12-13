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
    public class HomeView : FragmentView<IHomeView>, IHomeView
    {
        private const int StateSearchButton = 0;
        private const int StateLockList = 1;
        private const int StateLock = 2;


        private TextView _tvGreeting;
        private ImageView _ivMessage;

        private View _searchingBtnContainer;
        private ImageView _ivScanButton;
        private TextView _tvScanButton;
        private TextView _tvBtStatus;

        private RecyclerView _rvBleList;

        private View _lockContainer;
        private TextView _tvLockTitle;
        private TextView _tvLockSubTitle;
        private TextView _tvBatteryStatus;
        private SlideUnlockView _slideUnlockView;

        private bool isScanning;

        private BleDeviceAdapter _adapter;

        private bool _isOn;

        protected override int LayoutId => Resource.Layout.View_Home;

        public event Action<bool> StartStop;
        public event Action<BleDevice> Connect;
        public event Action CancelConnect;
        public event Action UnlockClicked;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _tvGreeting = _view.FindViewById<TextView>(Resource.Id.tvGreeting);
            _ivMessage = _view.FindViewById<ImageView>(Resource.Id.ivMessage);

            _searchingBtnContainer = _view.FindViewById<View>(Resource.Id.searchingBtnContainer);
            _ivScanButton = _view.FindViewById<ImageView>(Resource.Id.ivScanButton);
            _tvScanButton = _view.FindViewById<TextView>(Resource.Id.tvScanButton);
            _tvBtStatus = _view.FindViewById<TextView>(Resource.Id.tvBtStatus);

            _rvBleList = _view.FindViewById<RecyclerView>(Resource.Id.rvBleList);

            _lockContainer = _view.FindViewById<View>(Resource.Id.lockContainer);
            _tvLockTitle = _view.FindViewById<TextView>(Resource.Id.tvLockTitle);
            _tvLockSubTitle = _view.FindViewById<TextView>(Resource.Id.tvLockSubTitle);
            _tvBatteryStatus = _view.FindViewById<TextView>(Resource.Id.tvBatteryStatus);
            _slideUnlockView = _view.FindViewById<SlideUnlockView>(Resource.Id.SlideUnlockView);

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

            return _view;
        }

        public void Show(string greeting, bool btStatus)
        {
            _tvGreeting.Text = greeting;
            _tvBtStatus.Text = _isOn ? "ON" : "OFF";
        }

        public void Show(List<BleDevice> bleDevices)
        {
            SetMode(StateLockList);

            if (_adapter == null)
            {
                _adapter = new BleDeviceAdapter(bleDevices, Connect, CancelConnect);
                _rvBleList.SetLayoutManager(new LinearLayoutManager(Context));
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
            _ivMessage.Visibility = state == StateSearchButton ? ViewStates.Visible : ViewStates.Gone;
            _searchingBtnContainer.Visibility = state == StateSearchButton ? ViewStates.Visible : ViewStates.Gone;
            _rvBleList.Visibility = state == StateLockList ? ViewStates.Visible : ViewStates.Gone;
            _lockContainer.Visibility = state == StateLock ? ViewStates.Visible : ViewStates.Gone;
        }

        private void ToggleScanStatus(bool forceStop = false)
        {
            isScanning = !isScanning && !forceStop;

            _tvScanButton.Text = isScanning ? "Searching" : "Search Lock";
            _ivScanButton.SetImageResource(isScanning ? Resource.Drawable.searching_lock : Resource.Drawable.search_lock);
            ConfigureRotatingButton(isScanning);
        }

        private void ConfigureRotatingButton(bool start)
        {
            var anim = AnimationUtils.LoadAnimation(Context, Resource.Animation.anim_rotate);
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