using Android.App;
using Android.OS;
using Android.Views;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class MyLockView : FragmentView<IMyLockView>, IMyLockView
    {
        protected override int LayoutId => Resource.Layout.View_MyLock;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            return _view;
        }
    }
}