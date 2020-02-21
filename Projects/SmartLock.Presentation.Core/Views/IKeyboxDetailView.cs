using SmartLock.Model.Models;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxDetailView : IView
    {
        event Action BackClick;
        event Action LockHistoryClick;
        event Action LockDashboardClick;

        void Show(Keybox keybox);
    }
}
