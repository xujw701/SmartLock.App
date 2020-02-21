using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface ISettingView : IView
    {
        event Action ProfileClick;
        event Action PasswordClick;
        event Action FeedbackClick;
        event Action LogoutClick;

        void Show(string name);
    }
}
