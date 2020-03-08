using System;
using CoreGraphics;
using SmartLock.Presentation.iOS.Controls.CustomField;
using UIKit;

namespace SmartLock.Presentation.iOS.Support
{
    public static class UIHelper
    {
        public static void SetupPicker(DropdownField dropdownField, string[] items, Action<string> itemChanged)
        {
            var toolBar = new UIToolbar(new CGRect(0, 0, 320, 44));
            var flexibleSpaceLeft = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);
            var doneButton = new UIBarButtonItem("OK", UIBarButtonItemStyle.Done, (s, e) =>
            {
                dropdownField.EndEditing(true);
            });
            var list = new UIBarButtonItem[] { flexibleSpaceLeft, doneButton };
            toolBar.SetItems(list, false);

            UIPickerView pickerView = new UIPickerView(new CGRect(0, 44, 320, 216));
            pickerView.Model = new DropdownViewModel(dropdownField, items, itemChanged);
            pickerView.ShowSelectionIndicator = true;

            dropdownField.InputAccessoryView = toolBar;
            dropdownField.InputView = pickerView;
        }
    }
}
