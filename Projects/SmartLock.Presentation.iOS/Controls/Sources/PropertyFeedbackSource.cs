using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class PropertyFeedbackSource : UITableViewSource
    {
        public List<PropertyFeedback> PropertyFeedbacks;

        public PropertyFeedbackSource(List<PropertyFeedback> propertyFeedbacks)
        {
            PropertyFeedbacks = propertyFeedbacks;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var result = (PropertyFeedbackCell)tableView.DequeueReusableCell(PropertyFeedbackCell.Key) ?? PropertyFeedbackCell.Create();

            var propertyFeedback = PropertyFeedbacks[indexPath.Row];
            result.SetData(propertyFeedback);

            return result;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return PropertyFeedbacks.Count;
        }
    }
}