using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IMyProfileView : IView
    {
        event Action BackClick;
        event Action<string, string, string, string> SubmitClick;

        void Show(string firstName, string lastName, string email, string phone);
    }
}
