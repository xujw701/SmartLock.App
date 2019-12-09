using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using SmartLock.Infrastructure;
using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class PairingView : ViewBase<IPairingView>, IPairingView
    {
        protected override int LayoutId => Resource.Layout.View_Pairing;
        protected override bool SwipeRefresh => false;

        private RecyclerView _rvBleList;
        private TextView _btnStartStop;
        private Button _btnConnect;

        private Button _btnUnlock;
        private Button _btnLock;
        private Button _btnbBattery;

        private BleDeviceAdapter _adapter;

        private bool isScanning;

        public event Action<bool> StartStop;
        public event Action<BleDevice> Connect;

        private IBlueToothLeService BlueToothLeService => IoC.Resolve<IBlueToothLeService>();
        private ITrackedBleService TrackedBleService => IoC.Resolve<ITrackedBleService>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _rvBleList = FindViewById<RecyclerView>(Resource.Id.rvBleList);
            _btnStartStop = FindViewById<TextView>(Resource.Id.btnStartStop);
            _btnConnect = FindViewById<Button>(Resource.Id.btnConnect);

            _btnStartStop.Click += (s, e) =>
            {
                ToggleScanStatus();

                StartStop?.Invoke(isScanning);
            };

            _btnConnect.Click += (s, e) =>
            {
                ToggleScanStatus();

                Connect?.Invoke(_adapter.SelectedDevice);
            };

            _btnUnlock = FindViewById<Button>(Resource.Id.unlock);
            _btnLock = FindViewById<Button>(Resource.Id.btnLock);
            _btnbBattery = FindViewById<Button>(Resource.Id.battery);

            TrackedBleService.Init();

            _btnUnlock.Click += (s, a) => TrackedBleService.Unlock();
            _btnLock.Click += (s, a) => TrackedBleService.Lock();
            _btnbBattery.Click += (s, a) => BlueToothLeService.GetBatteryLevel();
        }

        public void Show(List<BleDevice> bleDevices)
        {
            if (_adapter == null)
            {
                _adapter = new BleDeviceAdapter(bleDevices);
                _rvBleList.SetLayoutManager(new LinearLayoutManager(this));
                _rvBleList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.BleDevices = bleDevices;
                _adapter.NotifyDataSetChanged();
            }
        }

        private void ToggleScanStatus()
        {
            isScanning = !isScanning;

            _btnStartStop.Text = isScanning ? "STOP" : "SCAN";
            IsBusy = isScanning;
        }
    }
}