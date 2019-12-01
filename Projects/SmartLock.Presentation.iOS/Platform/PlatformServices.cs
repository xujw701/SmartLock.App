using System.Text.RegularExpressions;
using SmartLock.Model.Services;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace SmartLock.Presentation.iOS.Platform
{
    public class PlatformServices :IPlatformServices
    {
        public void Call(string phoneNumber)
        {
            var phoneUrl =  new NSUrl("tel:" + Regex.Replace(phoneNumber, @"[^0-9]+", ""));
            UIApplication.SharedApplication.OpenUrl(phoneUrl);
        }

        public void Sms(string phoneNumber)
        {
            var smsUrl = NSUrl.FromString("sms:" + Regex.Replace(phoneNumber, @"[^0-9]+", ""));
            UIApplication.SharedApplication.OpenUrl(smsUrl);
        }

        public void Email(string emailAddress)
        {
            var emailUrl = NSUrl.FromString("mailto:" + emailAddress);
            UIApplication.SharedApplication.OpenUrl(emailUrl);
        }

        public void Exit()
        {
            UIApplication.SharedApplication.PerformSelector(new Selector("terminateWithSuccess"), null, 0f);
        }
    }
}