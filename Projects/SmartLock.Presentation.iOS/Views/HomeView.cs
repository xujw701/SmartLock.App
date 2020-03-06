using System;
using System.Collections.Generic;
using System.Timers;
using CoreAnimation;
using CoreGraphics;
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

        public event Action MessageClick;
        public event Action<bool> StartStop;
        public event Action<Keybox> Connect;
        public event Action<Keybox> Disconnect;
        public event Action DisconnectCurrent;
        public event Action UnlockClicked;
        public event Action BtClicked;

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
                DisconnectCurrent?.Invoke();
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

            DisconnectCurrent += () =>
            {
                SetMode(StateIdle);
            };
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

                if (_bleDeviceSource == null)
                {
                    _bleDeviceSource = new BleDeviceSource(keyboxes, Connect, Disconnect, DisconnectCurrent);

                    BleDeviceTableView.EstimatedRowHeight = 190f;
                    BleDeviceTableView.RowHeight = UITableView.AutomaticDimension;
                    BleDeviceTableView.Source = _bleDeviceSource;
                }
                else
                {
                    _bleDeviceSource.Keyboxes = keyboxes;
                }

                BleDeviceTableView.ReloadData();
            });
        }

        public void Show(Keybox keybox)
        {
            SetMode(StateLock);

            LblText1.Text = keybox.PropertyAddress;
            LblText2.Text = keybox.KeyboxName;
            LblBattery.Text = keybox.BatteryLevelString;

            ShadowHelper.AddShadow(LockContainer);
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

        public void UpdateTimeout(int second)
        {
            InvokeOnMainThread(() =>
            {
                LblTimeout.Text = $"Timeout: {second}s";
            });
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

