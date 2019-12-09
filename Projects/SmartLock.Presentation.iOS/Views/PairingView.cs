using System;
using System.Collections.Generic;
using CoreGraphics;
using SmartLock.Infrastructure;
using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Controls.Sources;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public class PairingView : TableView<IPairingView>, IPairingView
    {
        private UIActivityIndicatorView _loadingIndicator;
        private UIButton _startStopButton;

        private bool isScanning;
        private BleDeviceSource _bleDeviceSource;

        public event Action<bool> StartStop;
        public event Action<BleDevice> Connect;

        private IBlueToothLeService BlueToothLeService => IoC.Resolve<IBlueToothLeService>();
        private ITrackedBleService TrackedBleService => IoC.Resolve<ITrackedBleService>();

        public PairingView(PairingController controller) : base(controller)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            TrackedBleService.Init();
        }

        public void Show(List<BleDevice> bleDevices)
        {
            if (_bleDeviceSource == null)
            {
                _bleDeviceSource = new BleDeviceSource(bleDevices);

                TableView.TableHeaderView = CreateHeader();
                TableView.TableFooterView = CreateFooter();
                TableView.EstimatedRowHeight = 50f;
                TableView.RowHeight = UITableView.AutomaticDimension;
                TableView.Source = _bleDeviceSource;
            }
            else
            {
                _bleDeviceSource.BleDevices = bleDevices;
            }

            TableView.ReloadData();
        }

        protected override void OnIsBusyChanged()
        {
            if (IsBusy)
            {
                _loadingIndicator.StartAnimating();
            }
            else
            {
                _loadingIndicator.StopAnimating();
            }
        }

        private UIView CreateHeader()
        {
            var headerView = new UIView(new CGRect(0, 0, TableView.Frame.Width, 60));

            var labelRect = new CGRect(20, 20, 200, 20);
            var loadingIndicatorRect = new CGRect(240, 20, 20, 20);
            var startStopButtonRect = new CGRect(TableView.Frame.Width - 20 - 60, 20, 60, 20);

            var label = CreateLabel(labelRect, "LOCK DISCOVERED", 14);
            _loadingIndicator = CreateLoadingIndicator(loadingIndicatorRect);
            _startStopButton = CreateButton(startStopButtonRect, "SCAN", StartStopButtonClicked, true);

            headerView.AddSubview(label);
            headerView.AddSubview(_loadingIndicator);
            headerView.AddSubview(_startStopButton);

            return headerView;
        }

        private UIView CreateFooter()
        {
            var footerView = new UIView(new CGRect(0, 0, TableView.Frame.Width, 220));

            var margin = 50;

            var connectButtonRect = new CGRect(margin, 20, TableView.Frame.Width - margin * 2, 30);
            var unlockButtonRect = new CGRect(margin, 70, TableView.Frame.Width - margin * 2, 30);
            var lockButtonRect = new CGRect(margin, 120, TableView.Frame.Width - margin * 2, 30);
            var batteryButtonRect = new CGRect(margin, 170, TableView.Frame.Width - margin * 2, 30);

            var connectButton = CreateButton(connectButtonRect, "Connect", ConnectButtonClicked, false);
            var unlockButton = CreateButton(unlockButtonRect, "Unlock", () => { TrackedBleService.Unlock(); }, false);
            var lockButton = CreateButton(lockButtonRect, "Lock", () => { TrackedBleService.Lock(); }, false);
            var batteryLevelButton = CreateButton(batteryButtonRect, "Battery Level", () => { BlueToothLeService.GetBatteryLevel(); }, false);

            footerView.AddSubview(connectButton);
            footerView.AddSubview(unlockButton);
            footerView.AddSubview(lockButton);
            footerView.AddSubview(batteryLevelButton);

            return footerView;
        }

        private void StartStopButtonClicked()
        {
            ToggleScanStatus();

            StartStop?.Invoke(isScanning);
        }

        private void ConnectButtonClicked()
        {
            ToggleScanStatus();

            Connect?.Invoke(_bleDeviceSource.SelectedDevice);
        }

        private void ToggleScanStatus()
        {
            isScanning = !isScanning;

            _startStopButton.SetTitle(isScanning ? "STOP": "SCAN", UIControlState.Normal);

            IsBusy = isScanning;
        }
    }
}

