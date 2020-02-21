using SmartLock.Model.Models;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxesView : IView
    {
        event Action<Keybox> KeyboxClicked;
        event Action PlaceKeyboxClicked;

        void Show(List<Keybox> keyboxes, bool placeLockButtonEnabled);
        void UpdatePlaceLockButton(bool enabled);
    }
}
