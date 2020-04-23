using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IAttachmentView : IView
    {
        event Action BackClick;

        void Show(Cache cache);
    }
}