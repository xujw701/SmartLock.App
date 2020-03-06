using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class BleDeviceSource : UITableViewSource
    {
        private Action<Keybox> _connect;
        private Action<Keybox> _disconnect;
        private Action _cancel;

        public List<Keybox> Keyboxes;
        public Keybox ConnectedKeybox;

        public BleDeviceSource(List<Keybox> keyboxes, Action<Keybox> connect, Action<Keybox> disconnect, Action cancel)
        {
            Keyboxes = keyboxes;
            _connect = connect;
            _disconnect = disconnect;
            _cancel = cancel;

            _connect += (keybox) =>
            {
                ConnectedKeybox = keybox;
            };
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (BleDeviceCell)tableView.DequeueReusableCell(BleDeviceCell.Key) ?? BleDeviceCell.Create();

            result.SetData(Keyboxes[indexPath.Row], _connect, _disconnect, _cancel);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Keyboxes.Count;
        }
    }
}