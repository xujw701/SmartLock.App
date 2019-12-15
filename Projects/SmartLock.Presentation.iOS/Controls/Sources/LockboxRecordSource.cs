using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class LockboxRecordSource : UITableViewSource
    {
        public List<KeyboxHistory> LockboxRecords;

        public LockboxRecordSource(List<KeyboxHistory> lockboxRecords)
        {
            LockboxRecords = lockboxRecords;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (LockboxRecordCell)tableView.DequeueReusableCell(LockboxRecordCell.Key) ?? LockboxRecordCell.Create();

            var lockboxRecord = LockboxRecords[indexPath.Row];
            result.Configure(lockboxRecord.LockName, lockboxRecord.DateTimeString);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return LockboxRecords.Count;
        }
    }
}