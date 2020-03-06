using SmartLock.Presentation.iOS.Platform;
using UIKit;
using SmartLock.Infrastructure;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core;
using SmartLock.Presentation.Core.ViewService;
using SmartLock.Model.PushNotification;
using SmartLock.Presentation.iOS.PushNotification;

namespace SmartLock.Presentation.iOS
{
	public class Application : AppCore
	{
        public static Application Current { get; private set; }

        // This is the main entry point of the application.
        static void Main (string[] args)
		{
            // Setup the app core
            Current = new Application();

            // Start iOS app delegate
			UIApplication.Main (args, null, "AppDelegate");
		}

        protected override void RegisterPlatformServices()
        {
            IoC.RegisterSingleton<IViewService, ViewService>();
            IoC.RegisterSingleton<ISettingsService, SettingsService>();
            IoC.RegisterSingleton<ILocalBleService, LocalBleService>();
            IoC.RegisterSingleton<IDevicePushNotifications, DevicePushNotifications>();

            IoC.Register<IContainedStorage, ContainedStorage>();
            IoC.Register<IMessageBoxService, MessageBoxService>();
            IoC.Register<IPlatformServices, PlatformServices>();
        }
    }
}
