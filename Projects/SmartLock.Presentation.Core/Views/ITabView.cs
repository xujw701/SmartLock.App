using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface ITabView : IView
    {
        void ShowTabs(params object[] viewControllers);
    }
}
