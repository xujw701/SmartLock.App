using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public class MainView : TabView<IMainView>, IMainView
    {
        public MainView(MainController controller) : base(controller)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void SetTabs(HomeController homeController, KeyboxesController keyboxesController, /*ListingController listingController, NearbyController nearbyController,*/ SettingController settingController)
        {
            ViewControllers = new UIViewController[]
            {
                new UINavigationController(LoadView(homeController)),
                new UINavigationController(LoadView(keyboxesController)),
                //new UINavigationController(LoadView(listingController)),
                //new UINavigationController(LoadView(nearbyController)),
                new UINavigationController(LoadView(settingController))
            };

            TabBar.Items[0].Title = "Home";
            TabBar.Items[0].Image = UIImage.FromBundle("icon_home_nor");
            TabBar.Items[0].SelectedImage = UIImage.FromBundle("icon_home_sel");

            TabBar.Items[1].Title = "Keybox";
            TabBar.Items[1].Image = UIImage.FromBundle("icon_keboxes_nor");
            TabBar.Items[1].SelectedImage = UIImage.FromBundle("icon_keyboxes_sel");

            //TabBar.Items[2].Title = "Listing";
            //TabBar.Items[2].Image = UIImage.FromBundle("icon_listing_nor");
            //TabBar.Items[2].SelectedImage = UIImage.FromBundle("icon_listing_sel");

            //TabBar.Items[3].Title = "Nearby";
            //TabBar.Items[3].Image = UIImage.FromBundle("icon_nearby_nor");
            //TabBar.Items[3].SelectedImage = UIImage.FromBundle("icon_nearby_sel");

            TabBar.Items[2].Title = "Settings";
            TabBar.Items[2].Image = UIImage.FromBundle("icon_settings_nor");
            TabBar.Items[2].SelectedImage = UIImage.FromBundle("icon_settings_sel");
        }
    }
}

