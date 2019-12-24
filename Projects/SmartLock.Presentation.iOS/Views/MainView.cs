using System;
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

        public void SetTabs(HomeController homeController, KeyboxesController keyboxesController, ListingController listingController, NearbyController nearbyController, SettingController settingController)
        {
            ViewControllers = new UIViewController[]
            {
                new UINavigationController(LoadView(homeController)),
                new UINavigationController(LoadView(keyboxesController)),
                new UINavigationController(LoadView(listingController)),
                new UINavigationController(LoadView(nearbyController)),
                new UINavigationController(LoadView(settingController))
            };

            TabBar.Items[0].Title = "Home";
            TabBar.Items[0].Image = UIImage.FromBundle("home_black_24pt");
            //TabBar.Items[0].SelectedImage = UIImage.FromBundle("home_black_24pt");

            TabBar.Items[1].Title = "Keybox";
            TabBar.Items[1].Image = UIImage.FromBundle("lock_black_24pt");
            //TabBar.Items[1].SelectedImage = UIImage.FromBundle("lock_black_24pt");

            TabBar.Items[2].Title = "Listing";
            TabBar.Items[2].Image = UIImage.FromBundle("timeline_black_24pt");
            //TabBar.Items[2].SelectedImage = UIImage.FromBundle("timeline_black_24pt");

            TabBar.Items[3].Title = "Nearby";
            TabBar.Items[3].Image = UIImage.FromBundle("build_black_24pt");
            //TabBar.Items[3].SelectedImage = UIImage.FromBundle("build_black_24pt");

            TabBar.Items[4].Title = "Settings";
            TabBar.Items[4].Image = UIImage.FromBundle("build_black_24pt");
            //TabBar.Items[4].SelectedImage = UIImage.FromBundle("build_black_24pt");
        }
    }
}

