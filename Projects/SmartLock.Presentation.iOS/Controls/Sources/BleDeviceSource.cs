using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.Ble;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class BleDeviceSource : UITableViewSource
    {
        private Action<BleDevice> _connect;
        private Action<BleDevice> _disconnect;

        public List<BleDevice> BleDevices;
        public BleDevice ConnectedDevice;

        public BleDeviceSource(List<BleDevice> bleDevices, Action<BleDevice> connect, Action<BleDevice> disconnect)
        {
            BleDevices = bleDevices;
            _connect = connect;
            _disconnect = disconnect;

            _connect += (d) => ConnectedDevice = d;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (BleDeviceCell)tableView.DequeueReusableCell(BleDeviceCell.Key) ?? BleDeviceCell.Create();

            result.SetData(BleDevices[indexPath.Row], _connect, _disconnect);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return BleDevices.Count;
        }
    }
}