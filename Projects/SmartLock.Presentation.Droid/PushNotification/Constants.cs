using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SmartLock.Presentation.Droid.PushNotification
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://smartelockdev.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=GoGE6B+b2Kn9AXKv/KpYiGlNsRAn+gsDuIBSLWaRoPc=";
        public const string NotificationHubName = "SmartElockDevNotificationHub";
    }
}