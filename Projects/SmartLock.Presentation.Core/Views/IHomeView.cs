using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IHomeView : IView
    {
        event Action MessageClick;
        event Action PlaceKeyboxClicked;
        event Action<bool> StartStop;
        event Action<Keybox> Connect;
        event Action<Keybox> Cancel;
        event Action<Keybox> Dismiss;
        event Action Close;
        event Action UnlockClicked;
        event Action BtClicked;
        event Action Timeout;

        void Show(string greeting, string name, bool setMode = true);
        void Show(List<Keybox> keyboxes);
        void Show(Keybox keybox, bool showPlaceLock);
        void SetLockUI(bool locked);
        void SetBleIndicator(bool isOn);
        void StartCountDown(int timeout);
        void StopCountDown();
    }
}
