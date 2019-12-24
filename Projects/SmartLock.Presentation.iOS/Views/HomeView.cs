using System;
using System.Collections.Generic;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public class HomeView : TableView<IHomeView>, IHomeView
    {
        public event Action<bool> StartStop;
        public event Action<BleDevice> Connect;
        public event Action<BleDevice> Disconnect;
        public event Action DisconnectCurrent;
        public event Action UnlockClicked;

        public HomeView(HomeController controller) : base(controller)
        {
        }

        public void Show(string greeting, bool btStatuss, bool setMode = true)
        {
        }

        public void Show(List<BleDevice> bleDevices)
        {
        }

        public void Show(BleDevice bleDevice)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

