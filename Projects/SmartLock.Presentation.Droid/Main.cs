using Android.App;
using Android.Content.PM;
using Android.Support.V7.App;
using SmartLock.Presentation.Droid.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Droid
{
    /// <summary>
    /// Main launcher for displaying splash screen
    /// </summary>
	[Activity(Theme = "@style/SmartLockTheme.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
	public class Main : AppCompatActivity
    {
        protected override async void OnResume()
		{
			base.OnResume();

            if (ViewBase.CurrentActivity == null)
            {
                ViewBase.CurrentActivity = this;

                await Task.Delay(500);

                App.Current.Start();
            }
            else
            {
                App.Current.Resume();
            }
		}
    }
}