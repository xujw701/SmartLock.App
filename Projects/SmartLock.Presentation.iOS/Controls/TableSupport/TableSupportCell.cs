using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.TableSupport
{
    public abstract class TableSupportCell
    {
        /// <summary>
        /// Re-use ID (unique) to save on memory.
        /// </summary>
        protected abstract string CellReuseIdentifier { get; }

        /// <summary>
        /// Determines if the celll should be highlighted
        /// </summary>
        public virtual bool ShouldHighlight => false;

        /// <summary>
        /// Determines if the cell should deselect after <see cref="Selected"/>
        /// </summary>
        public virtual bool ShouldDeselectOnSelect => true;

        /// <summary>
        /// Determines if a cell can be edited
        /// </summary>
        public virtual bool CanEdit { get; set; }

        /// <summary>
        /// Raised when a cell is selected (<see cref="ShouldHighlight"/> must be true)
        /// </summary>
        public virtual void Selected(UITableView tableView, UITableViewCell cell)
        {

        }

        /// <summary>
        /// Logic to create or re-use a cell
        /// </summary>
        public UITableViewCell GetCell(TableSupportSource source, UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellReuseIdentifier) ?? CreateCell();

            UpdateCell(cell);
            UpdateCell(source, tableView, cell, indexPath);

            return cell;
        }

        /// <summary>
        /// Create a new cell.
        /// </summary>
        /// <returns></returns>
        protected abstract UITableViewCell CreateCell();

        /// <summary>
        /// Updates a cell.
        /// </summary>
        /// <param name="cell"></param>
        protected virtual void UpdateCell(UITableViewCell cell)
        {
        }

        /// <summary>
        /// Updates a cell.
        /// </summary>
        protected virtual void UpdateCell(TableSupportSource source, UITableView tableView, UITableViewCell cell, NSIndexPath path)
        {
        }

        public virtual bool IsVisible { get; set; } = true;
    }
}