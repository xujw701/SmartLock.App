using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface ILoginView : IView
    {
        event Action<string, string> LoginClicked;
    }
}
