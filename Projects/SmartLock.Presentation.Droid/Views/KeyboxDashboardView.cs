using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class KeyboxDashboardView : ViewBase<IKeyboxDashboardView>, IKeyboxDashboardView
    {
        private ImageView _btnBack;
        private ImageView _ivGraph;

        public event Action BackClick;

        protected override int LayoutId => Resource.Layout.View_KeyboxDashboard;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _ivGraph = FindViewById<ImageView>(Resource.Id.ivGraph);

            _btnBack.Click += (s, e) => BackClick?.Invoke();

            ConfigureGraph();
        }

        private void ConfigureGraph()
        {
            var displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            int width = displayMetrics.WidthPixels;

            var ivHeight = width / 1.399;
            _ivGraph.LayoutParameters.Height = (int)ivHeight;
            _ivGraph.RequestLayout();
        }
    }
}