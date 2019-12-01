using Android.App;
using Android.Content.PM;
using Android.Support.V7.App;
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
        protected async override void OnResume()
		{
			base.OnResume();

            ViewBase.CurrentActivity = this;

            await Task.Delay(1000);

            App.Current.Start();
		}
    }
}