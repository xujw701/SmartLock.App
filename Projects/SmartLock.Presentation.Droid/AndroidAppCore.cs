using System;
using SmartLock.Presentation.Droid.Platform;
using SmartLock.Infrastructure;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core;
using SmartLock.Presentation.Core.ViewService;
using System.Collections.Generic;
using Android.Views;
using SmartLock.Model.PushNotification;
using SmartLock.Presentation.Droid.PushNotification;

namespace SmartLock.Presentation.Droid
{
    public class AndroidAppCore : AppCore
    {
        public static bool? ConnectionState;
        public static List<Window> Windows;

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