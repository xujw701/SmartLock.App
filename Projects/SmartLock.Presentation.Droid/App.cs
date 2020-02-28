using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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

            AppCenter.Start("f643e23e-fe4a-4c37-bd51-98bd270d4018",
                   typeof(Analytics), typeof(Crashes));

            Current = new AndroidAppCore();

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
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