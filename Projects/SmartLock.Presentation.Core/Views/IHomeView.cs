using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IHomeView : IView
    {
        event Action<bool> StartStop;
        event Action<Keybox> Connect;
        event Action<Keybox> Disconnect;
        event Action DisconnectCurrent;
        event Action UnlockClicked;
        event Action BtClicked;

        void Show(string greeting, string name, bool setMode = true);
        void Show(List<Keybox> keyboxes);
        void Show(Keybox keybox);
        void Unlocked();
        void SetBleIndicator(bool isOn);
    }
}
