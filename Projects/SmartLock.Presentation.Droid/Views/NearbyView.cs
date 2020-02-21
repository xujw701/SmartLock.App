using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmartLock.Infrastructure;
using SmartLock.Model.Ble;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NearbyView : FragmentView<INearbyView>, INearbyView
    {
        private ImageView _ivGraph;

        protected override int LayoutId => Resource.Layout.View_Nearby;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _ivGraph = _view.FindViewById<ImageView>(Resource.Id.ivNearBy);

            ConfigureGraph();

            return _view;
        }

        private void ConfigureGraph()
        {
            var displayMetrics = new DisplayMetrics();
            ViewBase.CurrentActivity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            int width = displayMetrics.WidthPixels;

            var ivHeight = width / 0.5125;
            _ivGraph.LayoutParameters.Height = (int)ivHeight;
            _ivGraph.RequestLayout();
        }
    }
}