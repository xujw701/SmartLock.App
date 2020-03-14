using System;
using System.Collections.Generic;
using CoreAnimation;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Controls.Sources;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class HomeView : View<IHomeView>, IHomeView
    {
        private const int StateIdle = 0;
        private const int StateLockList = 1;
        private const int StateLock = 2;

        private BleDeviceSource _bleDeviceSource;
        private bool isScanning;

        private int _timeout = 0;
        private NSTimer _timer;

        protected override bool CanSwipeBack => false;

        public event Action MessageClick;
        public event Action<bool> StartStop;
        public event Action<Keybox> Connect;
        public event Action<Keybox> Cancel;
        public event Action<Keybox> Dismiss;
        public event Action Close;
        public event Action UnlockClicked;
        public event Action BtClicked;
        public event Action Timeout;

        public HomeView(HomeController controller) : base(controller, "HomeView")
        {
            FullscreenIsBusy = false;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvMessage.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                MessageClick?.Invoke();
            }));

            LblScanButton.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                ToggleScanStatus();

                StartStop?.Invoke(isScanning);
            }));

            IvClose.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                Close?.Invoke();
            }));

            UnlockSlider.Unlocked += () =>
            {
                UnlockClicked?.Invoke();
            };

            LblBt.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BtClicked?.Invoke();
            }));

            LblBtStatus.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BtClicked?.Invoke();
            }));
        }

        public void Show(string greeting, string name, bool setMode = true)
        {
            InvokeOnMainThread(() =>
            {
                if (setMode)
                {
                    SetMode(StateIdle);
                }

                LblGreeting.Text = greeting;
                LblName.Text = name;
            });
        }

        public void Show(List<Keybox> keyboxes)
        {
            InvokeOnMainThread(() =>
            {
                SetMode(StateLockList);

                _bleDeviceSource = new BleDeviceSource(keyboxes, Connect, Cancel, Dismiss);

                BleDeviceTableView.EstimatedRowHeight = 190f;
                BleDeviceTableView.RowHeight = UITableView.AutomaticDimension;
                BleDeviceTableView.Source = _bleDeviceSource;
                BleDeviceTableView.ReloadData();
            });
        }

        public void Show(Keybox keybox)
        {
            InvokeOnMainThread(() =>
            {
                SetMode(StateLock);

                LblText1.Text = keybox.PropertyAddress;
                LblText2.Text = keybox.KeyboxName;
                LblBattery.Text = keybox.BatteryLevelString;

                ShadowHelper.AddShadow(LockContainer);
            });
        }

        public void SetLockUI(bool locked)
        {
            InvokeOnMainThread(() =>
            {
                LblSliderText.Text = locked ? "         Slide to unlock" : "Unlocked";

                IvLockIcon.Image = locked ? UIImage.FromBundle("icon_lock_big") : UIImage.FromBundle("icon_unlock");

                if (locked)
                {
                    UnlockSlider.Reset();
                }
            });
        }

        public void SetBleIndicator(bool isOn)
        {
            LblBtStatus.Text = isOn ? "ON" : "OFF";
            LblBtStatus.TextColor = isOn ? UIColor.FromRGB(0, 194, 63) : UIColor.FromRGB(255, 28, 28);
        }

        public void StartCountDown(int timeout)
        {
            InvokeOnMainThread(() =>
            {
                _timeout = timeout;

                if (_timer != null)
                {
                    _timer.Invalidate();
                }

                _timer = NSTimer.CreateScheduledTimer(1, this, new ObjCRuntime.Selector("ShowCountDown:"), null, true);
                _timer.Fire();
            });
        }

        public void StopCountDown()
        {
            InvokeOnMainThread(() =>
            {
                if (_timer != null)
                {
                    _timer.Invalidate();
                }
            });
        }

        [Export("ShowCountDown:")]
        private void ShowCountDown(NSTimer timer)
        {
            if (_timeout > 0)
            {
                LblTimeout.Text = $"Reconnect remaining: {_timeout}s";
                --_timeout;
            }
            else
            {
                Timeout?.Invoke();
                timer.Invalidate();
            }
        }

        private void SetMode(int state)
        {
            IvScanButton.Hidden = state != StateIdle;
            LblScanButton.Hidden = state != StateIdle;
            IvBt.Hidden = state != StateIdle;
            LblBt.Hidden = state != StateIdle;
            LblBtStatus.Hidden = state != StateIdle;

            BleDeviceTableView.Hidden = state != StateLockList;

            LockContainer.Hidden = state != StateLock;

            if (state != StateIdle) ToggleScanStatus(true);
            if (state == StateLock) SetLockUI(true);
        }

        private void ToggleScanStatus(bool forceStop = false)
        {
            isScanning = !isScanning && !forceStop;

            LblScanButton.Text = isScanning ? "Searching" : "Search Lock";
            IvScanButton.Image = isScanning ? UIImage.FromBundle("searching_lock") : UIImage.FromBundle("search_lock");
            ConfigureRotatingButton(isScanning);
        }

        private void ConfigureRotatingButton(bool start)
        {
            if (start)
            {
                var rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
                rotationAnimation.To = NSNumber.FromDouble(Math.PI * 2); // full rotation (in radians)
                rotationAnimation.RepeatCount = int.MaxValue; // repeat forever
                rotationAnimation.Duration = 2;
                // Give the added animation a key for referencing it later (to remove, in this case).
                IvScanButton.Layer.AddAnimation(rotationAnimation, "ScanRotationAnimation");
            }
            else
            {
                IvScanButton.Layer.RemoveAllAnimations();
            }
        }
    }
}

