using System;
using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Support
{
    public static class SystemHelper
    {
        public static void OpenUrl(NSUrl nsurl)
        {
            if (GetSystemVersion() < 10)
            {
                UIApplication.SharedApplication.OpenUrl(nsurl);
            }
            else
            {
                UIApplication.SharedApplication.OpenUrl(nsurl,
                    new NSDictionary { }, null);
            }
        }

        public static int GetSystemVersion()
        {
            var version = 0;

            if (float.TryParse(UIDevice.CurrentDevice.SystemVersion, out var versionNumber))
            {
                version = (int)Math.Floor(versionNumber);
            }

            return version;
        }
    }
}
