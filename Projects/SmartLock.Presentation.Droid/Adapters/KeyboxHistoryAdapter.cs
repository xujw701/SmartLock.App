using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.BlueToothLe;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class KeyboxHistoryAdapter : RecyclerView.Adapter
    {
        private Action<KeyboxHistory> _keyboxSetOut;

        public List<KeyboxHistory> KeyboxHistories;

        public KeyboxHistoryAdapter(List<KeyboxHistory> keyboxHistories, Action<KeyboxHistory> keyboxSetOut)
        {
            KeyboxHistories = keyboxHistories;

            _keyboxSetOut = keyboxSetOut;
        }

        public override int ItemCount => KeyboxHistories.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.Item_KeyboxHistory, parent, false);
            var holder = new LockboxRecordHolder(parent.Context, KeyboxHistories, _keyboxSetOut, itemView);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is LockboxRecordHolder reviewHolder)
            {
                reviewHolder.SetData(KeyboxHistories[position]);
            }
        }

        // Holders definition
        public class LockboxRecordHolder : RecyclerView.ViewHolder
        {
            private readonly Context _context;

            private readonly List<KeyboxHistory> _keyboxHistories;
            private readonly Action<KeyboxHistory> _keyboxSetOut;

            private readonly View _container;
            private readonly ImageView _ivPortrait;
            private readonly TextView _tvName;
            private readonly TextView _tvDuration;
            private readonly TextView _tvIn;
            private readonly TextView _tvOut;
            private readonly View _btnOut;

            public LockboxRecordHolder(Context context, List<KeyboxHistory> keyboxHistories, Action<KeyboxHistory> keyboxSetOut, View itemView) : base(itemView)
            {
                _context = context;
                _keyboxHistories = keyboxHistories;
                _keyboxSetOut = keyboxSetOut;

                _container = itemView.FindViewById<View>(Resource.Id.container);

                _ivPortrait = itemView.FindViewById<ImageView>(Resource.Id.ivPortrait);
                _tvName = itemView.FindViewById<TextView>(Resource.Id.tvName);
                _tvDuration = itemView.FindViewById<TextView>(Resource.Id.tvDuration);
                _tvIn = itemView.FindViewById<TextView>(Resource.Id.tvIn);
                _tvOut = itemView.FindViewById<TextView>(Resource.Id.tvOut);
                _btnOut = itemView.FindViewById<View>(Resource.Id.btnOut);

                _btnOut.Click += (s, e) =>
                {
                    var history = keyboxHistories[AdapterPosition];
                    if (history.OutTime == null)
                    {
                        _keyboxSetOut?.Invoke(history);
                    }
                };
            }

            public void SetData(KeyboxHistory keyboxHistory)
            {
                _tvName.Text = keyboxHistory.Opener;
                _tvDuration.Text = keyboxHistory.Duration;
                _tvIn.Text = keyboxHistory.InTimeString;
                _tvOut.Text = keyboxHistory.OutTimeString;

                ConfigureDemoPortait(keyboxHistory);
            }

            private void ConfigureDemoPortait(KeyboxHistory keyboxHistory)
            {
                if (keyboxHistory.Opener.StartsWith("Della"))
                {
                    _ivPortrait.SetImageDrawable(_context.GetDrawable(Resource.Drawable.portait1));
                }
                else if (keyboxHistory.Opener.StartsWith("Mol"))
                {
                    _ivPortrait.SetImageDrawable(_context.GetDrawable(Resource.Drawable.portait3));
                }
                else if (keyboxHistory.Opener.StartsWith("Win"))
                {
                    _ivPortrait.SetImageDrawable(_context.GetDrawable(Resource.Drawable.portait5));
                }
                else if (keyboxHistory.Opener.StartsWith("Har"))
                {
                    _ivPortrait.SetImageDrawable(_context.GetDrawable(Resource.Drawable.portait2));
                }
                else
                {
                    _ivPortrait.SetImageDrawable(_context.GetDrawable(Resource.Drawable.portait4));
                }
            }
        }
    }
}