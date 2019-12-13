using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IHomeView : IView
    {
        event Action<bool> StartStop;
        event Action<BleDevice> Connect;
        event Action CancelConnect;
        event Action UnlockClicked;

        void Show(string greeting, bool btStatus);
        void Show(List<BleDevice> bleDevices);
        void Show(BleDevice bleDevice);
    }
}
