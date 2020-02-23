using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PropertyFeedbackView : ViewBase<IPropertyFeedbackView>, IPropertyFeedbackView
    {
        private ImageView _btnBack;

        private View _writeFeedbackContainer;
        private EditText _etFeedback;
        private Button _btnSend;
        private RecyclerView _rvPropertyFeedback;

        private PropertyFeedbackAdapter _adapter;

        public event Action BackClick;
        public event Action<string> SubmitClick;

        protected override int LayoutId => Resource.Layout.View_PropertyFeedback;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);

            _writeFeedbackContainer = FindViewById<View>(Resource.Id.writeFeedbackContainer);
            _etFeedback = FindViewById<EditText>(Resource.Id.etFeedback);
            _btnSend = FindViewById<Button>(Resource.Id.btnSend);
            _rvPropertyFeedback = FindViewById<RecyclerView>(Resource.Id.rvPropertyFeedback);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
            _btnSend.Click += (s, e) => SubmitClick?.Invoke(_etFeedback.Text);
        }

        public void Show()
        {
            _writeFeedbackContainer.Visibility = ViewStates.Visible;
            _rvPropertyFeedback.Visibility = ViewStates.Gone;
        }

        public void Show(List<PropertyFeedback> propertyFeedbacks)
        {
            _writeFeedbackContainer.Visibility = ViewStates.Gone;
            _rvPropertyFeedback.Visibility = ViewStates.Visible;

            if (_adapter == null)
            {
                _adapter = new PropertyFeedbackAdapter(propertyFeedbacks);
                _rvPropertyFeedback.SetLayoutManager(new LinearLayoutManager(this));
                _rvPropertyFeedback.SetAdapter(_adapter);
            }
            else
            {
                _adapter.PropertyFeedbacks = propertyFeedbacks;
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}