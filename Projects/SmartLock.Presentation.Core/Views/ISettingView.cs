using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface ISettingView : IView
    {
        event Action<byte[]> PortraitChanged;
        event Action ProfileClick;
        event Action PasswordClick;
        event Action FeedbackClick;
        event Action LogoutClick;
        event Action Refresh;

        void Show(string name, Cache portrait);
    }
}
