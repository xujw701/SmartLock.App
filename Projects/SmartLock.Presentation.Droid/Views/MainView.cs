using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainView : FragmentContainerView<IMainView>, IMainView
    {
        protected override int LayoutId => Resource.Layout.View_Main;
        protected override int FragmentContainerId => Resource.Id.content_frame;
        protected override bool BlockBackPress => true;

        private HomeController _homeController;
        private KeyboxesController _keyboxesController;
        private ListingController _listingController;
        private NearbyController _nearbyController;
        private SettingController _settingController;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Setup the navigation events
            var navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            navigationView.NavigationItemSelected += BottomNavigationButtonClicked;
            navigationView.ItemIconTintList = null;

            var states = new int[][]
            {
                new int[]{-Android.Resource.Attribute.StateChecked},
                new int[]{ Android.Resource.Attribute.StateChecked }
            };
            var colors = new int[]
            {
                ContextCompat.GetColor(this, Resource.Color.bottom_bar_gray),
                ContextCompat.GetColor(this, Resource.Color.bottom_bar_blue)
            };
            var csl = new ColorStateList(states, colors);
            navigationView.ItemTextColor = csl;
        }

        public void SetTabs(HomeController homeController, KeyboxesController keyboxesController, ListingController listingController, NearbyController nearbyController, SettingController settingController)
        {
            _homeController = homeController;
            _keyboxesController = keyboxesController;
            _listingController = listingController;
            _nearbyController = nearbyController;
            _settingController = settingController;

            // Set first fragment to display
            DisplayFragment(_homeController);
        }

        private void BottomNavigationButtonClicked(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            // Display fragment for the selected item
            switch (e.Item.ItemId)
            {
                case Resource.Id.nav_home:
                    DisplayFragment(_homeController);
                    break;
                case Resource.Id.nav_lockbox:
                    DisplayFragment(_keyboxesController);
                    break;
                case Resource.Id.nav_listing:
                    DisplayFragment(_listingController);
                    break;
                case Resource.Id.nav_nearby:
                    DisplayFragment(_nearbyController);
                    break;
                case Resource.Id.nav_setting:
                    DisplayFragment(_settingController);
                    break;
            }
        }
    }
}