using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IPairingView : IView
    {
        event Action<bool> StartStop;
        event Action<BleDevice> Connect;

        void Show(List<BleDevice> bleDevices);
    }
}
