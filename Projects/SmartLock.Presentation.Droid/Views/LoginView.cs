using Android.App;
using Android.OS;
using Android.Widget;
using SmartLock.Infrastructure;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class LoginView : ViewBase<ILoginView>, ILoginView
    {
        protected override int LayoutId => Resource.Layout.View_Login;

        protected override bool SwipeRefresh => false;

        private IBlueToothLeService BlueToothLeService => IoC.Resolve<IBlueToothLeService>();

        private Button _btnConnect;
        private Button _btnUnlock;
        private Button _btnLock;
        private Button _btnbBattery;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnConnect = FindViewById<Button>(Resource.Id.connect);
            _btnUnlock = FindViewById<Button>(Resource.Id.unlock);
            _btnLock = FindViewById<Button>(Resource.Id.btnLock);
            _btnbBattery = FindViewById<Button>(Resource.Id.battery);

            _btnConnect.Click += (s, a) => BlueToothLeService.StartScanningForDevicesAsync();
            _btnUnlock.Click += (s, a) => BlueToothLeService.SetLock(false);
            _btnLock.Click += (s, a) => BlueToothLeService.SetLock(true);
            _btnbBattery.Click += (s, a) => BlueToothLeService.GetBatteryLevel();
        }
    }
}