using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Controls;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxDashboardView : ViewBase<IKeyboxDashboardView>, IKeyboxDashboardView
    {
        private ImageView _btnBack;
        private ImageView _ivGraph;

        private Spinner _spinDay;
        private Spinner _spinWeather;
        private Spinner _spinGender;

        public event Action BackClick;

        protected override int LayoutId => Resource.Layout.View_KeyboxDashboard;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            
            _spinDay = FindViewById<Spinner>(Resource.Id.spinDay);
            _spinWeather = FindViewById<Spinner>(Resource.Id.spinWeather);
            _spinGender = FindViewById<Spinner>(Resource.Id.spinGender);

            _ivGraph = FindViewById<ImageView>(Resource.Id.ivGraph);

            _btnBack.Click += (s, e) => BackClick?.Invoke();

            ConfigueSpinner(_spinDay, "Dates", new List<string>()
            {
                "Weekday",
                "Weekend"
            });

            ConfigueSpinner(_spinWeather, "Weather", new List<string>()
            {
                "Sunny",
                "Windy",
                "Rainy"
            });

            ConfigueSpinner(_spinGender, "Gender", new List<string>()
            {
                "Male",
                "Female"
            });
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

        private void ConfigueSpinner(Spinner spinner, string title, List<string> items)
        {
            if (!items.Contains(title)) items.Add(title);

            var adapter = new CustomArrayAdapter(this, Resource.Layout.Item_Spinner_Text, items);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            spinner.Adapter = adapter;
            spinner.SetSelection(items.Count - 1, true);
        }
    }
}