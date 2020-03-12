using SmartLock.Model.Models;
using SmartLock.Model.Views;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.Views
{
    public interface IPropertyFeedbackView : IView
    {
        event Action BackClick;
        event Action<string> SubmitClick;

        void Show();
        void Show(List<PropertyFeedback> propertyFeedbacks);
    }
}
