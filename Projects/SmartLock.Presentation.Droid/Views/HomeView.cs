using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Controls;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class HomeView : FragmentView<IHomeView>, IHomeView
    {
        private const int StateIdle = 0;
        private const int StateLockList = 1;
        private const int StateLock = 2;

        private Context _context;

        private TextView _tvGreeting;
        private TextView _tvName;
        private ImageView _ivMessage;

        private View _searchingBtnContainer;
        private ImageView _ivScanButton;
        private TextView _tvScanButton;
        private TextView _tvBtStatus;

        private RecyclerView _rvBleList;

        private View _lockContainer;
        private ImageView _ivLockDisconnect;
        private ImageView _ivLock;
        private ImageView _ivUnlock;
        private TextView _tvLockTitle;
        private TextView _tvLockSubTitle;
        private TextView _tvBatteryStatus;
        private TextView _tvTimeOut;
        private SlideUnlockView _slideUnlockView;

        private bool isScanning;

        private BleDeviceAdapter _adapter;

        private Timer _timer;

        protected override int LayoutId => Resource.Layout.View_Home;

        public event Action MessageClick;
        public event Action<bool> StartStop;
        public event Action<Keybox> Connect;
        public event Action<Keybox> Cancel;
        public event Action<Keybox> Dismiss;
        public event Action Close;
        public event Action UnlockClicked;
        public event Action BtClicked;
        public event Action Timeout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _context = ViewBase.CurrentActivity;

            _tvGreeting = _view.FindViewById<TextView>(Resource.Id.tvGreeting);
            _tvName = _view.FindViewById<TextView>(Resource.Id.tvName);
            _ivMessage = _view.FindViewById<ImageView>(Resource.Id.ivMessage);

            _searchingBtnContainer = _view.FindViewById<View>(Resource.Id.searchingBtnContainer);
            _ivScanButton = _view.FindViewById<ImageView>(Resource.Id.ivScanButton);
            _tvScanButton = _view.FindViewById<TextView>(Resource.Id.tvScanButton);
            _tvBtStatus = _view.FindViewById<TextView>(Resource.Id.tvBtStatus);

            _rvBleList = _view.FindViewById<RecyclerView>(Resource.Id.rvBleList);

            _lockContainer = _view.FindViewById<View>(Resource.Id.lockContainer);
            _ivLockDisconnect = _view.FindViewById<ImageView>(Resource.Id.ivLockDisconnect);
            _ivLock = _view.FindViewById<ImageView>(Resource.Id.ivLock);
            _ivUnlock = _view.FindViewById<ImageView>(Resource.Id.ivUnlock);
            _tvLockTitle = _view.FindViewById<TextView>(Resource.Id.tvLockTitle);
            _tvLockSubTitle = _view.FindViewById<TextView>(Resource.Id.tvLockSubTitle);
            _tvBatteryStatus = _view.FindViewById<TextView>(Resource.Id.tvBatteryStatus);
            _tvTimeOut = _view.FindViewById<TextView>(Resource.Id.tvTimeOut);
            _slideUnlockView = _view.FindViewById<SlideUnlockView>(Resource.Id.SlideUnlockView);

            ConfigureScanButtonSize();

            _ivMessage.Click += (s, e) =>
            {
                MessageClick?.Invoke();
            };

            _ivScanButton.Click += (s, e) =>
            {
                ToggleScanStatus();

                StartStop?.Invoke(isScanning);
            };

            _ivLockDisconnect.Click += (s, e) =>
            {
                Close?.Invoke();
            };

            _slideUnlockView.Unlocked += () =>
            {
                UnlockClicked?.Invoke();
            };

            _tvBtStatus.Click += (s, e) =>
            {
                BtClicked?.Invoke();
            };

            return _view;
        }

        public void Show(string greeting, string name, bool setMode = true)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                if (setMode)
                {
                    SetMode(StateIdle);
                }

                _tvGreeting.Text = greeting;
                _tvName.Text = name;
            });
        }

        public void Show(List<Keybox> keyboxes)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                SetMode(StateLockList);

                _adapter = new BleDeviceAdapter(keyboxes, Connect, Cancel, Dismiss);
                _rvBleList.SetLayoutManager(new LinearLayoutManager(_context));
                _rvBleList.SetAdapter(_adapter);
                _adapter.NotifyDataSetChanged();
            });
        }

        public void Show(Keybox keybox)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                SetMode(StateLock);

                _tvLockTitle.Text = keybox.PropertyAddress;
                _tvLockSubTitle.Text = keybox.KeyboxName;
                _tvBatteryStatus.Text = keybox.BatteryLevelString;
            });
        }

        public void SetLockUI(bool locked)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                _ivLock.Visibility = locked ? ViewStates.Visible : ViewStates.Gone;
                _ivUnlock.Visibility = !locked ? ViewStates.Visible : ViewStates.Gone;

                if (locked)
                {
                    _slideUnlockView.Reset();
                }
            });
        }

        public void SetBleIndicator(bool isOn)
        {
            _tvBtStatus.Text = isOn ? "ON" : "OFF";
            _tvBtStatus.SetTextColor(new Color(_context.GetColor(isOn ? Resource.Color.bt_status_green : Resource.Color.bt_status_red)));
        }

        public void StartCountDown(int timeout)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                if (_timer != null)
                {
                    _timer.Cancel();
                }

                _timer = new Timer(_view, Timeout, timeout * 1000, 1000);

                _timer.Start();
            });
        }

        public void StopCountDown()
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                if (_timer != null)
                {
                    _timer.Cancel();
                }
            });
        }

        private void SetMode(int state)
        {
            _ivMessage.Visibility = state == StateIdle ? ViewStates.Visible : ViewStates.Gone;
            _searchingBtnContainer.Visibility = state == StateIdle ? ViewStates.Visible : ViewStates.Gone;
            _rvBleList.Visibility = state == StateLockList ? ViewStates.Visible : ViewStates.Gone;
            _lockContainer.Visibility = state == StateLock ? ViewStates.Visible : ViewStates.Gone;

            if (state != StateIdle) ToggleScanStatus(true);
            if (state == StateLock) SetLockUI(true);
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
            var anim = AnimationUtils.LoadAnimation(_context, Resource.Animation.anim_rotate);
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

        private void ConfigureScanButtonSize()
        {
            var displayMetrics = new DisplayMetrics();
            ViewBase.CurrentActivity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            int width = displayMetrics.WidthPixels;

            var ivWidth = width * 0.7333;
            _ivScanButton.LayoutParameters.Width = (int)ivWidth;
            _ivScanButton.LayoutParameters.Height = (int)ivWidth;
            _ivScanButton.RequestLayout();

            _tvScanButton.LayoutParameters.Width = (int)ivWidth;
            _tvScanButton.LayoutParameters.Height = (int)ivWidth;
            _tvScanButton.RequestLayout();
        }

        internal class Timer : CountDownTimer
        {
            private View _view;
            private TextView _tvTimeOut;
            private Action _timeout;

            public Timer(View view, Action timeout, long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval)
            {
                _view = view;
                _timeout = timeout;
                _tvTimeOut = _view.FindViewById<TextView>(Resource.Id.tvTimeOut);
            }

            public override void OnFinish()
            {
                _timeout?.Invoke();
            }

            public override void OnTick(long millisUntilFinished)
            {
                var second = (millisUntilFinished / 1000) + 1;
                _tvTimeOut.Text = $"Reconnect remaining: {second}s";
            }
        }
    }
}