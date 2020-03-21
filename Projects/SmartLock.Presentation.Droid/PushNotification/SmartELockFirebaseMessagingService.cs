using System.Linq;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using SmartLock.Presentation.Droid.Views;
using AndroidApp = Android.App;

namespace SmartLock.Presentation.Droid.PushNotification
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class SmartELockFirebaseMessagingService : FirebaseMessagingService
    {
        private const string CHANNEL_ID = "smartelock_notification_channel";

        public const string ParamFromNotification = "FromNotification";

        public override void OnMessageReceived(RemoteMessage message)
        {
            var titleKey = "title";
            var bodyKey = "body";
            var tagKey = "tag";
            var messageDate = "dateTime";

            var data = message.Data;

            if (data.ContainsKey(titleKey) && data.ContainsKey(bodyKey))
            {
                // These is how most messages will be received
                SendNotification(data[titleKey], data[bodyKey]);
            }
            else
            {
                // Only used for debugging payloads sent from the Azure portal
                SendNotification("Open Sez Me", message.Data.Values.First() ?? "Test Content");
            }
        }

        private void SendNotification(string title, string messageBody)
        {
            var context = AndroidApp.Application.Context;

            Intent notificationIntent = null;

            notificationIntent = new Intent(context, typeof(LoginView));
            notificationIntent.PutExtra(ParamFromNotification, true);

            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(LoginView)));
            stackBuilder.AddNextIntent(notificationIntent);

            var pendingIntent = PendingIntent.GetActivity(context, 1, notificationIntent, PendingIntentFlags.UpdateCurrent);
            var pendingIntentFullScreen = PendingIntent.GetActivity(context, 1, notificationIntent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context)
                        .SetSmallIcon(Resource.Drawable.ic_launcher)
                        .SetColor(Resource.Color.harcourts_blue)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        .SetContentTitle(title)
                        .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                        .SetContentText(messageBody)
                        .SetAutoCancel(true)
                        .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                notificationBuilder.SetFullScreenIntent(pendingIntentFullScreen, true);
            }

            var notificationManager = NotificationManager.FromContext(context);

            CreateNotificationChannel(notificationBuilder);

            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private void CreateNotificationChannel(NotificationCompat.Builder builder)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = CHANNEL_ID;
            var channelDescription = string.Empty;
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.High)
            {
                Description = channelDescription
            };

            var notificationManager = NotificationManager.FromContext(AndroidApp.Application.Context);
            notificationManager.CreateNotificationChannel(channel);
            builder.SetChannelId(CHANNEL_ID);
        }
    }
}