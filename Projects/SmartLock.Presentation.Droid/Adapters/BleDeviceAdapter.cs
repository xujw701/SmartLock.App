using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Ble;
using SmartLock.Model.Models;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class BleDeviceAdapter : RecyclerView.Adapter
    {
        private const int ItemHeader = 0;
        private const int ItemBleDevice = 1;

        private Action<Keybox> _connect;
        private Action<Keybox> _disconnect;
        private Action _cancel;

        public List<Keybox> Keyboxes;
        public Keybox ConnectedKeybox;

        public BleDeviceAdapter(List<Keybox> keyboxes, Action<Keybox> connect, Action<Keybox> disconnect, Action cancel)
        {
            Keyboxes = keyboxes;
            _connect = connect;
            _disconnect = disconnect;
            _cancel = cancel;

            _connect += (keybox) =>
            {
                ConnectedKeybox = keybox;
            };
        }

        public override int ItemCount => Keyboxes.Count + 1;

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
                return new BleDeviceHolder(Keyboxes, _connect, _disconnect, _cancel, itemView);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is HeaderHolder headerHolder)
            {
                headerHolder.SetData(Keyboxes.Count);
            }
            else if (holder is BleDeviceHolder reviewHolder)
            {
                reviewHolder.SetData(Keyboxes[position - 1]);
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
            private readonly List<Keybox> _keyboxes;

            private readonly TextView _tvTitle;
            private readonly TextView _tvSubTitle;
            private readonly TextView _tvBatteryStatus;
            private readonly Button _btnConnect;
            private readonly Button _btnCancel;
            private readonly ImageView _ivClose;

            public BleDeviceHolder(List<Keybox> keyboxes, Action<Keybox> connect, Action<Keybox> disconnect, Action cancel, View itemView) : base(itemView)
            {
                _keyboxes = keyboxes;

                _tvTitle = itemView.FindViewById<TextView>(Resource.Id.tvTitle);
                _tvSubTitle = itemView.FindViewById<TextView>(Resource.Id.tvSubTitle);
                _tvBatteryStatus = itemView.FindViewById<TextView>(Resource.Id.tvBatteryStatus);
                _btnConnect = itemView.FindViewById<Button>(Resource.Id.btnConnect);
                _btnCancel = itemView.FindViewById<Button>(Resource.Id.btnCancel);
                _ivClose = itemView.FindViewById<ImageView>(Resource.Id.ivClose);

                _btnConnect.Click += (s, e) =>
                {
                    UpdateUI(true);
                    connect?.Invoke(_keyboxes[AdapterPosition - 1]);
                };

                _btnCancel.Click += (s, e) =>
                {
                    UpdateUI(false);
                    disconnect?.Invoke(_keyboxes[AdapterPosition - 1]);
                };

                _ivClose.Click += (s, e) =>
                {
                    cancel?.Invoke();
                };
            }

            public void SetData(Keybox keybox)
            {
                _tvTitle.Text = keybox.PropertyAddress;
                _tvSubTitle.Text = keybox.KeyboxName;
                _tvBatteryStatus.Text = keybox.BatteryLevelString;

                UpdateUI(keybox.State == DeviceState.Connecting);
            }

            private void UpdateUI(bool connecting)
            {
                _btnConnect.Text = connecting ? "Connecting..." : "Connect";
                _btnCancel.Visibility = connecting ? ViewStates.Visible : ViewStates.Invisible;
            }
        }
    }
}