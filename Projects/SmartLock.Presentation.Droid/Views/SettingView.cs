using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingView : FragmentView<ISettingView>, ISettingView
    {
        protected override int LayoutId => Resource.Layout.View_Setting;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            return _view;
        }
    }
}