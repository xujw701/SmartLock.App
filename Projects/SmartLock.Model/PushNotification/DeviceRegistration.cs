using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Model.PushNotification
{
    public class DeviceRegistration
    {
        public const string Droid = "fcm";
        public const string IOS = "apns";

        public string Platform { get; set; }
        public string Handle { get; set; }
        public string[] Tags { get; set; }
    }
}
