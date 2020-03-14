using System;
using System.Runtime.InteropServices;
using Foundation;
using SmartLock.Model.PushNotification;
using WindowsAzure.Messaging;

namespace SmartLock.Presentation.iOS.PushNotification
{
    public class DevicePushNotifications : IDevicePushNotifications
    {
        private const string PushDeviceTokenKey = "PushDeviceToken";

        private SBNotificationHub _hub;

        private event Action<string> _deviceTokenObtained;

        public DeviceRegistration GetDeviceRegistration()
        {
            var deviceRegistration = new DeviceRegistration()
            {
                Platform = DeviceRegistration.IOS
            };
            return deviceRegistration;
        }

        public void BindDeviceTokenListener(Action<string> deviceTokenObtained)
        {
            _deviceTokenObtained = deviceTokenObtained;
        }

        public void ObtainDeviceToken(object token)
        {
            var deviceToken = (NSData)token;

            var deviceTokenString = deviceToken.Description;

            if (!string.IsNullOrWhiteSpace(deviceTokenString))
            {
                var result = new byte[deviceToken.Length];
                Marshal.Copy(deviceToken.Bytes, result, 0, (int)deviceToken.Length);
                deviceTokenString = BitConverter.ToString(result).Replace("-", "");

                //deviceTokenString = deviceTokenString.Trim('<').Trim('>').Replace(" ", "");
                //NSUserDefaults.StandardUserDefaults.SetString(deviceTokenString, PushDeviceTokenKey);

                _deviceTokenObtained?.Invoke(deviceTokenString);
            }

//#if DEBUG
//            _hub = new SBNotificationHub(Constants.ListenConnectionString, Constants.NotificationHubName);
//            //_hub.UnregisterAllAsync(deviceToken);
//            _hub.RegisterNativeAsync(deviceToken, null);
//#endif
        }
    }
}
