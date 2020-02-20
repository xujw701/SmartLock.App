using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Model.PushNotification
{
    public interface IDevicePushNotifications
    {
        DeviceRegistration GetDeviceRegistration();
        void BindDeviceTokenListener(Action<string> deviceTokenObtained);
        void ObtainDeviceToken(object token);
    }
}
