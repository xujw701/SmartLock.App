using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EnvironmentView : ViewBase<IEnvironmentView>, IEnvironmentView
    {
        private ImageView _btnBack;

        private TextView _tvAppVersion;
        private Spinner _spinEnvironment;

        public event Action BackClick;
        public event Action<string> EnvironemntChanged;

        protected override int LayoutId => Resource.Layout.View_Environment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _tvAppVersion = FindViewById<TextView>(Resource.Id.tvAppVersion);
            _spinEnvironment = FindViewById<Spinner>(Resource.Id.spinEnvironment);

            _btnBack.Click += (s, e) => BackClick?.Invoke();

            _spinEnvironment.ItemSelected += (s, e) =>
            {
                EnvironemntChanged?.Invoke(_spinEnvironment.SelectedItem.ToString());
            };
        }

        public void Show(List<string> environemnts, string selectedEnvironment)
        {
#if DEBUG
            var buildType = " (DEV)";
#else
            var buildType = " (PRD)";
#endif
            _tvAppVersion.Text = "App Version: " + Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName + buildType;

            ConfigueSpinner(_spinEnvironment, environemnts, selectedEnvironment);
        }

        private void ConfigueSpinner(Spinner spinner, List<string> items, string selectedItem)
        {
            var adapter = new ArrayAdapter(this, Resource.Layout.Item_Spinner_Text, items);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            spinner.Adapter = adapter;
            spinner.SetSelection(items.IndexOf(selectedItem), true);
        }
    }
}