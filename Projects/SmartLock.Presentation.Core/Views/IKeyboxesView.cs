using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxesView : IView
    {
        void Show(List<Keybox> keyboxes);
    }
}
