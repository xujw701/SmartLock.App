using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;

namespace SmartLock.Presentation.Droid.PushNotification
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class SmartELockFirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
        }
    }
}