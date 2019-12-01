using SmartLock.Presentation.Core.ViewControllers;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IMainView : IView
    {
        void SetTabs(HomeController homeViewController, MyLockController myLockViewController, LogsController logsViewController, SettingController settingViewController);
    }
}
