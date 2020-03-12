using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxHistoryView : IView
    {
        event Action BackClick;

        void Show(List<KeyboxHistory> keyboxHistories);
    }
}
