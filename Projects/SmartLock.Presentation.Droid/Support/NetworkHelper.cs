using Android.Content;
using Android.Net;

namespace SmartLock.Presentation.Droid.Support
{
    public static class NetworkHelper
    {
        private const int NetworkNone = -1;

        private const int NetworkMobile = 0;

        private const int NetworkWifi = 1;

        public static int GetNetWorkState(Context context)
        {  
            var connectivityManager = (ConnectivityManager)context
                .GetSystemService(Context.ConnectivityService);

            var activeNetworkInfo = connectivityManager
                .ActiveNetworkInfo;
            if (activeNetworkInfo != null && activeNetworkInfo.IsConnected)
            {
                switch (activeNetworkInfo.Type)
                {
                    case ConnectivityType.Wifi:
                        return NetworkWifi;
                    case ConnectivityType.Mobile:
                        return NetworkMobile;
                }
            }
            return NetworkNone;
        }

        public static bool Connected(int netWorkType)
        {
            switch (netWorkType)
            {
                case NetworkMobile:
                case NetworkWifi:
                    return true;
            }
            return false;
        }
    }
}