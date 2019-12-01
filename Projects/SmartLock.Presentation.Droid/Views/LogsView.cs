using Android.App;
using Android.OS;
using Android.Views;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class LogsView : FragmentView<ILogsView>, ILogsView
    {
        protected override int LayoutId => Resource.Layout.View_Logs;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            return _view;
        }
    }
}