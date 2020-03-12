using System;
using SmartLock.Model.Views;

namespace SmartLock.Presentation.Core.Views
{
    public interface ILoginView : IView
    {
        event Action<string, string> LoginClicked;
        event Action<bool> RememberMeClicked;

        void Show(bool rememberMe);
    }
}
