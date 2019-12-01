using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        private static UIApplication _app;
        // class-level declarations

        /// <summary>
        /// The navigation controller to be used by the view service
        /// </summary>
        internal static UINavigationController NavigationController { get; private set; }

        public override UIWindow Window { get; set; }

        public static UIColor StatusBarBgColor { get; set; }

        public static bool IsIpad = UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var shouldPerformAdditionalDelegateHandling = true;

            _app = app;

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Window.BackgroundColor = UIColor.White;
            Window.RootViewController = NavigationController = new UINavigationController();

            Application.Current.Start();

            Window.MakeKeyAndVisible();

            return shouldPerformAdditionalDelegateHandling;
        }

        public static void SetWhiteStatusBar()
        {
            _app.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
        }

        public static void SetBlackStatusBar()
        {
            _app.SetStatusBarStyle(UIStatusBarStyle.Default, false);
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


