using Android.App;
using Android.Content;
using Android.Views;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Content;

namespace SmartLock.Presentation.Droid.Views.ViewBases
{
    /// <summary>
    /// Used for context specific actions when casting to ViewBase{IView} is not possible
    /// Do not inherit directly for views, use <see cref="ViewBase{TView}"/>
    /// </summary>
    public abstract class ActivityBase : AppCompatActivity
    {
        public delegate void OnActivityResultHandler(int requestCode, Result resultCode, Intent data);
        public event OnActivityResultHandler OnActivityResultCalled;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            OnActivityResultCalled?.Invoke(requestCode, resultCode, data);
        }

        protected void CreateOptionsMenu(IMenu menu, int resource)
        {
            // inflate resource
            MenuInflater?.Inflate(resource, menu);
        }
    }
}