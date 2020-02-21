using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.Ble;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class KeyboxSource : UITableViewSource
    {
        private Action<Keybox> _keyboxClicked;

        public List<Keybox> Keyboxes;

        public KeyboxSource(List<Keybox> keyboxes, Action<Keybox> keyboxClicked)
        {
            Keyboxes = keyboxes;

            _keyboxClicked = keyboxClicked;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (KeyboxCell)tableView.DequeueReusableCell(KeyboxCell.Key) ?? KeyboxCell.Create();

            var keybox = Keyboxes[indexPath.Row];
            result.SetData(keybox);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _keyboxClicked?.Invoke(Keyboxes[indexPath.Row]);

            tableView.DeselectRow(indexPath, true);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Keyboxes.Count;
        }
    }
}