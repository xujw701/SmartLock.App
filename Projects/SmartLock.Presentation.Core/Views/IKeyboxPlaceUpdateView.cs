﻿using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IKeyboxPlaceUpdateView : IView
    {
        event Action BackClick;
        event Action<Property> SubmitClick;

        void Show(Keybox keybox, Property property);
    }
}