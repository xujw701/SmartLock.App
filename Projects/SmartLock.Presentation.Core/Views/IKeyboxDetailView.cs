using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxDetailView : IView
    {
        event Action BackClick;
        event Action LockHistoryClick;
        event Action LockEditClick;
        event Action LockDashboardClick;
        event Action LockDataClick;
        event Action FeedbackClick;
        event Action Refresh;
        event Action<Cache> ImageClick;

        void Show(Keybox keybox, Property property, bool mine);
    }
}
