using System;
using Android.App;
using Android.OS;
using Android.Runtime;

namespace SmartLock.Presentation.Droid
{
	[Application]
	public class App : Application
    {
		public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{

		}

		public static AndroidAppCore Current { get; set; }

        public override void OnCreate()
		{
			base.OnCreate();
			Current = new AndroidAppCore();
		}

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {

        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }
    }
}