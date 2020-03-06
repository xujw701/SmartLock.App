using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class KeyboxHistorySource : UITableViewSource
    {
        public List<KeyboxHistory> KeyboxHistories;

        public KeyboxHistorySource(List<KeyboxHistory> keyboxHistories)
        {
            KeyboxHistories = keyboxHistories;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (KeyboxHistoryCell)tableView.DequeueReusableCell(KeyboxHistoryCell.Key) ?? KeyboxHistoryCell.Create();

            var keyboxHistory = KeyboxHistories[indexPath.Row];
            result.SetData(keyboxHistory);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return KeyboxHistories.Count;
        }
    }
}