using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SmartLock.Infrastructure;
using SmartLock.Model.PushNotification;
using UIKit;
using UserNotifications;

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

            AppCenter.Start("32b3bdca-9328-4f0d-bebf-39baec3ac0b7",
                   typeof(Analytics), typeof(Crashes));

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request notification permissions from the user
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (approved, err) => {
                    // Handle approval
                });

            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Sound,
                                                                        (granted, error) =>
                {
                    if (granted)
                        InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            if (options != null && options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
            {
                ProcessNotification((NSDictionary)options[UIApplication.LaunchOptionsRemoteNotificationKey], false);
            }

            return shouldPerformAdditionalDelegateHandling;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            IoC.Resolve<IDevicePushNotifications>().ObtainDeviceToken(deviceToken);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
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

        private void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                var alertTitle = string.Empty;
                var alertBody = string.Empty;
                var alertTag = string.Empty;
                var messageDate = string.Empty;

                //Extract the alert text
                // NOTE: If you're using the simple alert by just specifying
                // "  aps:{alert:"alert msg here"}  ", this will work fine.
                // But if you're using a complex alert with Localization keys, etc.,
                // your "alert" object from the aps dictionary will be another NSDictionary.
                // Basically the JSON gets dumped right into a NSDictionary,
                // so keep that in mind.
                var alertKey = new NSString("alert");
                var titleKey = new NSString("title");
                var bodyKey = new NSString("body");
                var tagKey = new NSString("tag");
                var dateKey = new NSString("dateTime");
                if (aps.ContainsKey(alertKey))
                {
                    var alert = aps[alertKey] as NSDictionary;
                    if (alert.ContainsKey(titleKey))
                    {
                        alertTitle = (alert[titleKey] as NSString).ToString();
                    }
                    if (alert.ContainsKey(bodyKey))
                    {
                        alertBody = (alert[bodyKey] as NSString).ToString();
                    }
                    if (alert.ContainsKey(tagKey))
                    {
                        alertTag = (alert[tagKey] as NSString).ToString();
                    }
                    if (alert.ContainsKey(dateKey))
                    {
                        messageDate = (alert[dateKey] as NSString).ToString();
                    }
                }
                if (!fromFinishedLaunching)
                {
                    //if (UIApplication.SharedApplication.ApplicationState.Equals(UIApplicationState.Active))
                    //{
                    //    var avAlert = new UIAlertView
                    //    {
                    //        Title = alertTitle,
                    //        Message = alertBody
                    //    };
                    //    avAlert.AddButton("Read detail");
                    //    avAlert.AddButton("Dismiss");
                    //    avAlert.Show();

                    //    avAlert.Clicked += (s, args) =>
                    //    {
                    //    };
                    //}
                    //else
                    //{
                    //}
                }
            }
        }
    }
}


