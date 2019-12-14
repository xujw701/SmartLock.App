using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
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
    public class NearbyView : FragmentView<INearbyView>, INearbyView
    {
        protected override int LayoutId => Resource.Layout.View_Nearby;

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
        public event Action ViewLogs;

        private IBlueToothLeService BlueToothLeService => IoC.Resolve<IBlueToothLeService>();
        private ITrackedBleService TrackedBleService => IoC.Resolve<ITrackedBleService>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _rvBleList = _view.FindViewById<RecyclerView>(Resource.Id.rvBleList);
            _btnStartStop = _view.FindViewById<TextView>(Resource.Id.btnStartStop);
            _btnConnect = _view.FindViewById<Button>(Resource.Id.btnConnect);

            _btnStartStop.Click += (s, e) =>
            {
                ToggleScanStatus();

                StartStop?.Invoke(isScanning);
            };

            _btnConnect.Click += (s, e) =>
            {
                ToggleScanStatus();

                //Connect?.Invoke(_adapter.SelectedDevice);
            };

            _btnUnlock = _view.FindViewById<Button>(Resource.Id.unlock);
            _btnLock = _view.FindViewById<Button>(Resource.Id.btnLock);
            _btnbBattery = _view.FindViewById<Button>(Resource.Id.battery);

            TrackedBleService.Init();

            _btnUnlock.Click += (s, a) => TrackedBleService.Unlock();
            _btnLock.Click += (s, a) => TrackedBleService.Lock();
            _btnbBattery.Click += (s, a) => { /*BlueToothLeService.GetBatteryLevel();*/ ViewLogs?.Invoke(); };

            return _view;
        }

        public void Show(List<BleDevice> bleDevices)
        {
            //if (_adapter == null)
            //{
            //    //_adapter = new BleDeviceAdapter(bleDevices);
            //    _rvBleList.SetLayoutManager(new LinearLayoutManager(Context));
            //    _rvBleList.SetAdapter(_adapter);
            //}
            //else
            //{
            //    _adapter.BleDevices = bleDevices;
            //    _adapter.NotifyDataSetChanged();
            //}
        }

        private void ToggleScanStatus()
        {
            isScanning = !isScanning;

            _btnStartStop.Text = isScanning ? "STOP" : "SCAN";
            IsBusy = isScanning;
        }
    }
}