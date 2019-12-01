using System;
using System.Collections.Generic;
using Assura.Model.Timezone;
using Assura.Presentation.Core.PageField;

namespace Assura.Presentation.Core.Views.Custom
{
    public interface ICustomView : INavigationView
    {
        void Show(IEnumerable<FieldGroup> fields);

        bool ShowMandatoryHint { get; set; }

        bool TimeZoneVisible { get; set; }

        string TimeZoneText { get; set; }

        event EventHandler TimeZoneClicked;

        int? Layout { get; set; }

        int[] Widths { get; set; }
    }
}
