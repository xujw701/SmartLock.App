using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class KeyboxAdapter : RecyclerView.Adapter
    {
        private Action<Keybox> _keyboxClicked;

        public List<Keybox> Keyboxes;

        public KeyboxAdapter(List<Keybox> keyboxes, Action<Keybox> keyboxClicked)
        {
            Keyboxes = keyboxes;

            _keyboxClicked = keyboxClicked;
        }

        public override int ItemCount => Keyboxes.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.Item_Keybox, parent, false);
            var holder = new KeyboxHolder(parent.Context, Keyboxes, _keyboxClicked, itemView);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is KeyboxHolder reviewHolder)
            {
                reviewHolder.SetData(Keyboxes[position]);
            }
        }

        // Holders definition
        public class KeyboxHolder : RecyclerView.ViewHolder
        {
            private readonly Context _context;

            private readonly List<Keybox> _keyboxs;

            private readonly View _container;
            private readonly View _batteryContainer;
            private readonly View _datetimeContainer;

            private readonly TextView _tvText1;
            private readonly TextView _tvText2;
            private readonly TextView _tvBattery;
            private readonly TextView _tvDate;
            private readonly TextView _tvTime;

            public KeyboxHolder(Context context, List<Keybox> keyboxs, Action<Keybox> keyboxClicked, View itemView) : base(itemView)
            {
                _context = context;
                _keyboxs = keyboxs;

                _container = itemView.FindViewById<View>(Resource.Id.container);
                _batteryContainer = itemView.FindViewById<View>(Resource.Id.batteryContainer);
                _datetimeContainer = itemView.FindViewById<View>(Resource.Id.timeContainer);
                _tvText1 = itemView.FindViewById<TextView>(Resource.Id.tvText1);
                _tvText2 = itemView.FindViewById<TextView>(Resource.Id.tvText2);
                _tvBattery = itemView.FindViewById<TextView>(Resource.Id.tvBattery);
                _tvDate = itemView.FindViewById<TextView>(Resource.Id.tvDate);
                _tvTime = itemView.FindViewById<TextView>(Resource.Id.tvTime);

                _container.Click += (s, e) =>
                {
                    keyboxClicked?.Invoke(_keyboxs[AdapterPosition]);
                };
            }

            public void SetData(Keybox keybox)
            {
                _tvText1.Text = keybox.PropertyAddress;
                _tvText2.Text = keybox.KeyboxName;
                _tvBattery.Text = keybox.BatteryLevelString;

                if (keybox.InOn == null || keybox.InOn == DateTimeOffset.MinValue)
                {
                    _batteryContainer.Visibility = ViewStates.Visible;
                    _datetimeContainer.Visibility = ViewStates.Gone;
                }
                else
                {
                    _datetimeContainer.Visibility = ViewStates.Visible;
                    _batteryContainer.Visibility = ViewStates.Gone;

                    _tvDate.Text = keybox.InOnDateString;
                    _tvTime.Text = keybox.InOnTimeString;
                }

                SetBatteryColor(keybox);
            }

            private void SetBatteryColor(Keybox keybox)
            {
                if (keybox.BatteryLevel < 20)
                {
                    _tvBattery.SetTextColor(new Color(_context.GetColor(Resource.Color.bt_status_red)));
                }
                else if (keybox.BatteryLevel < 40)
                {
                    _tvBattery.SetTextColor(new Color(_context.GetColor(Resource.Color.bt_status_orange)));
                }
                else
                {
                    _tvBattery.SetTextColor(new Color(_context.GetColor(Resource.Color.bt_status_green)));
                }
            }
        }
    }
}