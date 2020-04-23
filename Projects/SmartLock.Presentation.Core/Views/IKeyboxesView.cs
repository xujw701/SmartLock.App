using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxesView : IView
    {
        event Action<Keybox> KeyboxClicked;
        event Action PlaceKeyboxClicked;
        event Action<bool> TabClicked;
        event Action Refresh;

        void Show(List<Keybox> keyboxes);
        void UpdatePlaceLockButton(string buttonText, bool enabled);
    }
}
