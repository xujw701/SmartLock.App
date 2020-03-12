using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IPasswordView : IView
    {
        event Action BackClick;
        event Action<string, string, string> SubmitClick;
    }
}
