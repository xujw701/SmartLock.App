using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxPlaceUpdateView : IView
    {
        event Action BackClick;
        event Action<byte[]> AttachmentAdded;
        event Action<Cache> AttachmentClicked;
        event Action<Cache> AttachmentDeleted;
        event Action<Property> SubmitClick;

        void Show(Keybox keybox, Property property);
    }
}