using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", LaunchMode = LaunchMode.SingleTask)]
    public class MainView : FragmentContainerView<IMainView>, IMainView
    {
        protected override int LayoutId => Resource.Layout.View_Main;
        protected override int FragmentContainerId => Resource.Id.content_frame;
        protected override bool BlockBackPress => true;

        private HomeController _homeViewController;
        private MyLockController _myLockViewController;
        private LogsController _logsViewController;
        private SettingController _settingViewController;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Setup the navigation events
            var navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            navigationView.NavigationItemSelected += BottomNavigationButtonClicked;
        }

        public void SetTabs(HomeController homeViewController, MyLockController myLockViewController, LogsController logsViewController, SettingController settingViewController)
        {
            _homeViewController = homeViewController;
            _myLockViewController = myLockViewController;
            _logsViewController = logsViewController;
            _settingViewController = settingViewController;

            // Set first fragment to display
            DisplayFragment(_homeViewController);
        }

        private void BottomNavigationButtonClicked(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            // Display fragment for the selected item
            switch (e.Item.ItemId)
            {
                case Resource.Id.nav_home:
                    DisplayFragment(_homeViewController);
                    break;
                case Resource.Id.nav_lock:
                    DisplayFragment(_myLockViewController);
                    break;
                case Resource.Id.nav_log:
                    DisplayFragment(_logsViewController);
                    break;
                case Resource.Id.nav_setting:
                    DisplayFragment(_settingViewController);
                    break;
            }
        }
    }
}