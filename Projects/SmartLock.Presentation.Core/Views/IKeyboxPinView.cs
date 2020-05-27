using SmartLock.Model.Views;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxPinView : IView
    {
        event Action BackClick;
        event Action<string, string, string> SubmitClick;
        event Action<bool> PinChanged;

        void OnPinChanged(bool success);
    }
}
