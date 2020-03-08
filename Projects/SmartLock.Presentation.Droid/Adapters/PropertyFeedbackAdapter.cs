using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Infrastructure;
using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class PropertyFeedbackAdapter : RecyclerView.Adapter
    {
        public List<PropertyFeedback> PropertyFeedbacks;

        public PropertyFeedbackAdapter(List<PropertyFeedback> propertyFeedbacks)
        {
            PropertyFeedbacks = propertyFeedbacks;
        }

        public override int ItemCount => PropertyFeedbacks.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var itemView = inflater.Inflate(Resource.Layout.Item_PropertyFeedback, parent, false);
            var holder = new PropertyFeedbackHolder(parent.Context, PropertyFeedbacks, itemView);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is PropertyFeedbackHolder propertyFeedbackHolder)
            {
                propertyFeedbackHolder.SetData(PropertyFeedbacks[position]);
            }
        }

        // Holders definition
        public class PropertyFeedbackHolder : RecyclerView.ViewHolder
        {
            private readonly Context _context;

            private readonly List<PropertyFeedback> _propertyFeedbacks;

            private readonly ImageView _ivPortrait;

            private readonly TextView _tvKeyboxName;
            private readonly TextView _tvName;
            private readonly TextView _tvDateTime;
            private readonly TextView _tvNotes;

            private readonly Button _btnPhone;
            private readonly Button _btnSms;

            public PropertyFeedbackHolder(Context context, List<PropertyFeedback> propertyFeedbacks, View itemView) : base(itemView)
            {
                _context = context;

                _propertyFeedbacks = propertyFeedbacks;

                _ivPortrait = itemView.FindViewById<ImageView>(Resource.Id.ivPortrait);

                _tvKeyboxName = itemView.FindViewById<TextView>(Resource.Id.tvKeyboxName);
                _tvName = itemView.FindViewById<TextView>(Resource.Id.tvName);
                _tvDateTime = itemView.FindViewById<TextView>(Resource.Id.tvDateTime);
                _tvNotes = itemView.FindViewById<TextView>(Resource.Id.tvNotes);

                _btnPhone = itemView.FindViewById<Button>(Resource.Id.btnPhone);
                _btnSms = itemView.FindViewById<Button>(Resource.Id.btnSms);

                _btnPhone.Click += (s, e) =>
                {
                    IoC.Resolve<IPlatformServices>().Call(_propertyFeedbacks[AdapterPosition].Phone);
                };

                _btnSms.Click += (s, e) =>
                {
                    IoC.Resolve<IPlatformServices>().Sms(_propertyFeedbacks[AdapterPosition].Phone);
                };
            }

            public void SetData(PropertyFeedback propertyFeedback)
            {
                _tvKeyboxName.Text = propertyFeedback.Address;
                _tvName.Text = propertyFeedback.Name;
                _tvDateTime.Text = propertyFeedback.CreatedOnString;
                _tvNotes.Text = propertyFeedback.Content;

                ConfigurePortait(propertyFeedback);
            }

            private void ConfigurePortait(PropertyFeedback propertyFeedback)
            {
                if (propertyFeedback.ResPortraitId.HasValue)
                {
                    ImageHelper.SetImageView(_ivPortrait, propertyFeedback.Portrait);
                }
            }
        }
    }
}