using System;
using System.Collections.Generic;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.CustomField
{
    public class DropdownViewModel : UIPickerViewModel
    {
        private readonly string[] _items;
        private readonly DropdownField _dropdownField;
        private readonly Action _callback;
        private readonly Action<string> _callback2;

        public DropdownViewModel(DropdownField dropdownField, string[] items, Action<string> selectedIndexChangeCallback)
        {
            _dropdownField = dropdownField;

            var displayItems = new List<string>(items);
            _items = displayItems.ToArray();

            _callback2 = selectedIndexChangeCallback;
        }

        public DropdownViewModel(DropdownField dropdownField, string[] items, Action selectedIndexChangeCallback)
        {
            _dropdownField = dropdownField;

            var displayItems = new List<string>(items);
            displayItems.Insert(0, "");
            _items = displayItems.ToArray();

            _callback = selectedIndexChangeCallback;
        }

        public override nint GetComponentCount(UIPickerView v)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _items.Length;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            switch (component)
            {
                case 0:
                    return _items[row];
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            _dropdownField.Text = _items[picker.SelectedRowInComponent(0)];
            _callback?.Invoke();
            _callback2?.Invoke(_items[picker.SelectedRowInComponent(0)]);
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            return 310f;
        }
    }
}
