using System;
using SmartLock.Model.Views;

namespace SmartLock.Presentation.Core.Views
{
    public interface IAboutView : IView
    {
        event Action FeedbackClick;
    }
}
