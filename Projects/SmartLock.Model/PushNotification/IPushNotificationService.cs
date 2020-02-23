using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Model.PushNotification
{
    public interface IPushNotificationService
    {
        /// <summary>
        /// Bind device token listener
        /// </summary>
        void BindDeviceTokenListener();
        /// <summary>
        /// Register to recieve notification hub notifications
        /// </summary>
        void Register();
        /// <summary>
        /// Unregister to recieve notification hub notifications
        /// </summary>
        void Unregister();
        /// <summary>
        /// Reregiser to clear the location tag
        /// </summary>
        void Reregiser();

        Task UnregisterAsync();
    }
}
