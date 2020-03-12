using System;
using SmartLock.Model.Views;

namespace SmartLock.Presentation.Core.Views
{
    public interface ITabView : IView
    {
        void ShowTabs(params object[] viewControllers);
    }
}
