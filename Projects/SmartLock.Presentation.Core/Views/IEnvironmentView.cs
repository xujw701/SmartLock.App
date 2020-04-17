using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IEnvironmentView : IView
    {
        event Action BackClick;
        event Action<string> EnvironemntChanged;

        void Show(List<string> environemnts, string selectedEnvironment);
    }
}
