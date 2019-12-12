using System;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class MainView : TabView<IMainView>, IMainView
    {
        public MainView(MainController controller) : base(controller)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void SetTabs(HomeController homeViewController, KeyboxesController myLockViewController, ListingController logsViewController, SettingController settingViewController)
        {
            ViewControllers = new UIViewController[]
            {
                new UINavigationController(LoadView(homeViewController)),
                new UINavigationController(LoadView(myLockViewController)),
                new UINavigationController(LoadView(logsViewController)),
                new UINavigationController(LoadView(settingViewController))
            };

            TabBar.Items[0].Title = "HOME";
            TabBar.Items[0].Image = UIImage.FromBundle("home_black_24pt");
            //TabBar.Items[0].SelectedImage = UIImage.FromBundle("home_black_24pt");

            TabBar.Items[1].Title = "MY LOCKS";
            TabBar.Items[1].Image = UIImage.FromBundle("lock_black_24pt");
            //TabBar.Items[1].SelectedImage = UIImage.FromBundle("lock_black_24pt");

            TabBar.Items[2].Title = "LOGS";
            TabBar.Items[2].Image = UIImage.FromBundle("timeline_black_24pt");
            //TabBar.Items[2].SelectedImage = UIImage.FromBundle("timeline_black_24pt");

            TabBar.Items[3].Title = "SETTINGS";
            TabBar.Items[3].Image = UIImage.FromBundle("build_black_24pt");
            //TabBar.Items[3].SelectedImage = UIImage.FromBundle("build_black_24pt");
        }
    }
}

