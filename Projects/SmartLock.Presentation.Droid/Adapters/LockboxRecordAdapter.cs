using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.BlueToothLe;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class LockboxRecordAdapter : RecyclerView.Adapter
    {
        public List<LockboxRecord> LockboxRecords;

        public LockboxRecordAdapter(List<LockboxRecord> lockboxRecords)
        {
            LockboxRecords = lockboxRecords;
        }

        public override int ItemCount => LockboxRecords.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.Item_LockboxRecord, parent, false);
            var holder = new LockboxRecordHolder(parent.Context, LockboxRecords, (lockboxRecord) => { }, itemView);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is LockboxRecordHolder reviewHolder)
            {
                reviewHolder.SetData(LockboxRecords[position]);
            }
        }

        // Holders definition
        public class LockboxRecordHolder : RecyclerView.ViewHolder
        {
            private readonly Context _context;

            private readonly List<LockboxRecord> _lockboxRecords;
            private readonly Action<LockboxRecord> _lockboxRecordSelected;

            private readonly View _container;
            private readonly TextView _tvText1;
            private readonly TextView _tvText2;

            public LockboxRecordHolder(Context context, List<LockboxRecord> lockboxRecords, Action<LockboxRecord> lockboxRecordSelected, View itemView) : base(itemView)
            {
                _context = context;
                _lockboxRecords = lockboxRecords;
                _lockboxRecordSelected = lockboxRecordSelected;

                _container = itemView.FindViewById<View>(Resource.Id.container);
                _tvText1 = itemView.FindViewById<TextView>(Resource.Id.tvText1);
                _tvText2 = itemView.FindViewById<TextView>(Resource.Id.tvText2);

                _container.Click += (s, e) =>
                {
                    _lockboxRecordSelected?.Invoke(lockboxRecords[AdapterPosition]);
                };
            }

            public void SetData(LockboxRecord lockboxRecord)
            {
                _tvText1.Text = lockboxRecord.LockName;
                _tvText2.Text = lockboxRecord.DateTimeString;
            }
        }
    }
}