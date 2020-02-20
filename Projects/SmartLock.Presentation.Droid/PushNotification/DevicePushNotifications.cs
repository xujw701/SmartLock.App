using System;
using Android.Content;
using Android.Gms.Common;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Firebase.Iid;
using SmartLock.Model.PushNotification;
using SmartLock.Presentation.Droid.Views.ViewBases;
using WindowsAzure.Messaging;

namespace SmartLock.Presentation.Droid.PushNotification
{
    public class DevicePushNotifications : IDevicePushNotifications
    {
        private Context _context;
        private NotificationHub _hub;

        public DevicePushNotifications()
        {
            _context = ViewBase.CurrentActivity;
            _hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, _context);
        }

        public DeviceRegistration GetDeviceRegistration()
        {
            if (IsPlayServicesAvailable())
            {
                var deviceRegistration = new DeviceRegistration()
                {
                    Platform = DeviceRegistration.Droid,
                    Handle = FirebaseInstanceId.Instance.Token
                };

                return deviceRegistration;
            }
            throw new Exception("Please install Play Services to receive push notifications.");
        }

        public void BindDeviceTokenListener(Action<string> deviceTokenObtained)
        {
        }

        public void ObtainDeviceToken(object token)
        {
        }

        private bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(_context);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Log.Info("DevicePushNotifications", "Google play service unavailable: " + GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else
                {
                    Log.Info("DevicePushNotifications", "Google play service unavailable, this device is not supported");
                }
                return false;
            }
            else
            {
                // google play services are available
                return true;
            }
        }
    }
}