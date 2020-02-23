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
    public class FeedbackView : ViewBase<IFeedbackView>, IFeedbackView
    {
        private ImageView _btnBack;

        private EditText _etFeedback;
        private Button _btnSend;

        public event Action BackClick;
        public event Action<string> SubmitClick;

        protected override int LayoutId => Resource.Layout.View_Feedback;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);

            _etFeedback = FindViewById<EditText>(Resource.Id.etFeedback);
            _btnSend = FindViewById<Button>(Resource.Id.btnSend);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
            _btnSend.Click += (s, e) => SubmitClick?.Invoke(_etFeedback.Text);
        }
    }
}