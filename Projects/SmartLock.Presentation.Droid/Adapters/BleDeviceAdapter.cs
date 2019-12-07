using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.BlueToothLe;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class BleDeviceAdapter : RecyclerView.Adapter
    {
        public List<BleDevice> BleDevices;

        public BleDevice SelectedDevice;

        public BleDeviceAdapter(List<BleDevice> bleDevices)
        {
            BleDevices = bleDevices;
        }

        public override int ItemCount => BleDevices.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.Item_BleDevice, parent, false);
            var holder = new BleDeviceHolder(parent.Context, BleDevices, (bleDevice) => { SelectedDevice = bleDevice; NotifyDataSetChanged(); }, itemView);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is BleDeviceHolder reviewHolder)
            {
                reviewHolder.SetData(BleDevices[position]);
            }
        }

        // Holders definition
        public class BleDeviceHolder : RecyclerView.ViewHolder
        {
            private readonly Context _context;

            private readonly List<BleDevice> _bleDevices;
            private readonly Action<BleDevice> _bleDeviceSelected;

            private readonly View _container;
            private readonly TextView _tvTitle;
            private readonly ImageView _ivCheck;

            private static BleDevice _selectedDevice;

            public BleDeviceHolder(Context context, List<BleDevice> bleDevices, Action<BleDevice> bleDeviceSelected, View itemView) : base(itemView)
            {
                _context = context;
                _bleDevices = bleDevices;
                _bleDeviceSelected = bleDeviceSelected;

                _container = itemView.FindViewById<View>(Resource.Id.container);
                _tvTitle = itemView.FindViewById<TextView>(Resource.Id.tvTitle);
                _ivCheck = itemView.FindViewById<ImageView>(Resource.Id.ivCheck);

                _container.Click += (s, e) =>
                {
                    _selectedDevice = _bleDevices[AdapterPosition];

                    _bleDeviceSelected?.Invoke(_selectedDevice);

                    UpdateCheckStatus(_selectedDevice);
                };
            }

            public void SetData(BleDevice bleDevice)
            {
                _tvTitle.Text = bleDevice.Name;

                UpdateCheckStatus(bleDevice);
            }

            private void UpdateCheckStatus(BleDevice bleDevice)
            {
                _ivCheck.Visibility = _selectedDevice == bleDevice ? ViewStates.Visible : ViewStates.Invisible;
            }
        }
    }
}