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
        private const int ItemHeader = 0;
        private const int ItemBleDevice = 1;

        private Action<BleDevice> _connect;
        private Action<BleDevice> _disconnect;

        public List<BleDevice> BleDevices;
        public BleDevice ConnectedDevice;

        public BleDeviceAdapter(List<BleDevice> bleDevices, Action<BleDevice> connect, Action<BleDevice> disconnect)
        {
            BleDevices = bleDevices;
            _connect = connect;
            _disconnect = disconnect;

            _connect += (d) => ConnectedDevice = d;
        }

        public override int ItemCount => BleDevices.Count + 1;

        public override int GetItemViewType(int position)
        {
            return position == 0 ? ItemHeader : ItemBleDevice;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);

            if (viewType == ItemHeader)
            {
                var headerView = inflater.Inflate(Resource.Layout.Item_BleDevice_Header, parent, false);
                return new HeaderHolder(headerView);
            }
            else
            {
                var itemView = inflater.Inflate(Resource.Layout.Item_BleDevice, parent, false);
                return new BleDeviceHolder(BleDevices, _connect, _disconnect, itemView);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is HeaderHolder headerHolder)
            {
                headerHolder.SetData(BleDevices.Count);
            }
            else if (holder is BleDeviceHolder reviewHolder)
            {
                reviewHolder.SetData(BleDevices[position - 1]);
            }
        }

        // Holders definition
        public class HeaderHolder : RecyclerView.ViewHolder
        {
            private readonly TextView _tvTitle;

            public HeaderHolder(View itemView) : base(itemView)
            {
                _tvTitle = itemView.FindViewById<TextView>(Resource.Id.tvTitle);
            }

            public void SetData(int count)
            {
                _tvTitle.Text = count > 1 ? $"{count} Locks Found" : $"{count} Lock Found";
            }
        }

        public class BleDeviceHolder : RecyclerView.ViewHolder
        {
            private readonly List<BleDevice> _bleDevices;

            private readonly TextView _tvTitle;
            private readonly TextView _tvSubTitle;
            private readonly Button _btnConnect;
            private readonly Button _btnCancel;

            public BleDeviceHolder(List<BleDevice> bleDevices, Action<BleDevice> connect, Action<BleDevice> disconnect, View itemView) : base(itemView)
            {
                _bleDevices = bleDevices;

                _tvTitle = itemView.FindViewById<TextView>(Resource.Id.tvTitle);
                _tvSubTitle = itemView.FindViewById<TextView>(Resource.Id.tvSubTitle);
                _btnConnect = itemView.FindViewById<Button>(Resource.Id.btnConnect);
                _btnCancel = itemView.FindViewById<Button>(Resource.Id.btnCancel);

                _btnConnect.Click += (s, e) =>
                {
                    UpdateUI(true);
                    connect?.Invoke(_bleDevices[AdapterPosition - 1]);
                };

                _btnCancel.Click += (s, e) =>
                {
                    UpdateUI(true);
                    disconnect?.Invoke(_bleDevices[AdapterPosition - 1]);
                };
            }

            public void SetData(BleDevice bleDevice)
            {
                _tvTitle.Text = bleDevice.Name;

                UpdateUI(bleDevice.State == DeviceState.Connecting);
            }

            private void UpdateUI(bool connecting)
            {
                _btnConnect.Text = connecting ? "Connecting..." : "Connect";
                _btnCancel.Visibility = connecting ? ViewStates.Visible : ViewStates.Invisible;
            }
        }
    }
}