using SmartLock.Model.Ble;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxHistoryView : IView
    {
        event Action BackClick;
        event Action<KeyboxHistory> KeyboxSetOut;

        void Show(List<KeyboxHistory> keyboxHistories);
    }
}
