using System;
using System.Collections.Generic;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Controls.Sources;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public class LogsView : TableView<IListingView>, IListingView
    {
        private LockboxRecordSource _lockboxRecordSource;

        public LogsView(ListingController controller) : base(controller)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void Show(List<KeyboxHistory> lockboxRecords)
        {
            if (_lockboxRecordSource == null)
            {
                _lockboxRecordSource = new LockboxRecordSource(lockboxRecords);
                TableView.EstimatedRowHeight = 50f;
                TableView.RowHeight = UITableView.AutomaticDimension;
                TableView.Source = _lockboxRecordSource;
            }
            else
            {
                _lockboxRecordSource.LockboxRecords = lockboxRecords;
            }

            TableView.ReloadData();
        }
    }
}

