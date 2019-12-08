using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class BleDeviceSource : UITableViewSource
    {
        public List<BleDevice> BleDevices;
        public BleDevice SelectedDevice;

        public BleDeviceSource(List<BleDevice> bleDevices)
        {
            BleDevices = bleDevices;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (BleDeviceCell)tableView.DequeueReusableCell(BleDeviceCell.Key) ?? BleDeviceCell.Create();

            var bleDevice = BleDevices[indexPath.Row];
            result.Configure(bleDevice.Name, bleDevice == SelectedDevice);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            SelectedDevice = BleDevices[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            tableView.ReloadData();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return BleDevices.Count;
        }
    }
}