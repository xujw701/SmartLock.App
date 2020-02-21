using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class KeyboxHistoryAdapter : RecyclerView.Adapter
    {
        public List<KeyboxHistory> KeyboxHistories;

        public KeyboxHistoryAdapter(List<KeyboxHistory> keyboxHistories)
        {
            KeyboxHistories = keyboxHistories;
        }

        public override int ItemCount => KeyboxHistories.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.Item_KeyboxHistory, parent, false);
            var holder = new LockboxRecordHolder(parent.Context, KeyboxHistories, itemView);
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

            private readonly View _container;
            private readonly ImageView _ivPortrait;
            private readonly TextView _tvName;
            private readonly TextView _tvDuration;
            private readonly TextView _tvIn;
            private readonly TextView _tvOut;
            private readonly View _btnOut;

            public LockboxRecordHolder(Context context, List<KeyboxHistory> keyboxHistories, View itemView) : base(itemView)
            {
                _context = context;
                _keyboxHistories = keyboxHistories;

                _container = itemView.FindViewById<View>(Resource.Id.container);

                _ivPortrait = itemView.FindViewById<ImageView>(Resource.Id.ivPortrait);
                _tvName = itemView.FindViewById<TextView>(Resource.Id.tvName);
                _tvDuration = itemView.FindViewById<TextView>(Resource.Id.tvDuration);
                _tvIn = itemView.FindViewById<TextView>(Resource.Id.tvIn);
                _tvOut = itemView.FindViewById<TextView>(Resource.Id.tvOut);
                _btnOut = itemView.FindViewById<View>(Resource.Id.btnOut);
            }

            public void SetData(KeyboxHistory keyboxHistory)
            {
                _tvName.Text = keyboxHistory.Name;
                _tvDuration.Text = keyboxHistory.Duration;
                _tvIn.Text = keyboxHistory.InOnString;
                _tvOut.Text = keyboxHistory.OutOnString;

                ConfigureDemoPortait(keyboxHistory);
            }

            private void ConfigureDemoPortait(KeyboxHistory keyboxHistory)
            {
                _ivPortrait.SetImageDrawable(_context.GetDrawable(Resource.Drawable.portait4));
            }
        }
    }
}