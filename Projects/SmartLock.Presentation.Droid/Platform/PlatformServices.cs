using Android.Content;
using SmartLock.Infrastructure;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Platform
{
    public class PlatformServices : IPlatformServices
    {
        public void Call(string phoneNumber)
        {
            var uri = Android.Net.Uri.Parse(string.Concat("tel:", phoneNumber));
            var intent = new Intent(Intent.ActionDial, uri);
            if (intent.ResolveActivity(ViewBase.CurrentActivity.PackageManager) != null)
            {
                ViewBase.CurrentActivity.StartActivity(intent);
            }
            else
            {
                IoC.Resolve<IMessageBoxService>().ShowMessage("Service unavailable", "This phone is unable to make a call.");
            }
        }

        public void Sms(string phoneNumber)
        {
            var smsUri = Android.Net.Uri.Parse(string.Concat("smsto:", phoneNumber));
            var intent = new Intent(Intent.ActionSendto, smsUri);

            // intent.PutExtra("sms_body", "Hello from Xamarin.Android"); // use this to put default text in SMS
            if (intent.ResolveActivity(ViewBase.CurrentActivity.PackageManager) != null)
            {
                ViewBase.CurrentActivity.StartActivity(intent);
            }
            else
            {
                IoC.Resolve<IMessageBoxService>().ShowMessage("Service unavailable", "This phone is unable to send SMS.");
            }
        }

        public void Email(string emailAddress)
        {

            var smsUri = Android.Net.Uri.Parse(string.Concat("mailto:", emailAddress));
            var intent = new Intent(Intent.ActionSendto, smsUri);

            // intent.PutExtra("sms_body", "Hello from Xamarin.Android"); // use this to put default text in SMS
            if (intent.ResolveActivity(ViewBase.CurrentActivity.PackageManager) != null)
            {
                ViewBase.CurrentActivity.StartActivity(intent);
            }
            else
            {
                IoC.Resolve<IMessageBoxService>().ShowMessage("Service unavailable", "This phone is unable to send email.");
            }
        }

        public void Bt()
        {
            var intent = new Intent();
            intent.SetAction(Android.Provider.Settings.ActionBluetoothSettings);
            if (intent.ResolveActivity(ViewBase.CurrentActivity.PackageManager) != null)
            {
                ViewBase.CurrentActivity.StartActivity(intent);
            }
            else
            {
                IoC.Resolve<IMessageBoxService>().ShowMessage("Service unavailable", "Cannot open bluetooth settings.");
            }
        }

        public void Exit()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}