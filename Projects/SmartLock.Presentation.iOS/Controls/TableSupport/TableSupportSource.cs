using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.TableSupport
{
    public class TableSupportSource : UITableViewSource
    {
        private TableSupportCellGroup[] _cellsGroups;
        private readonly Dictionary<NSIndexPath, nfloat> _heights = new Dictionary<NSIndexPath, nfloat>();
        private UITableView _tableView;

        public TableSupportSource(TableSupportCellGroup[] cellsGroups)
        {
            _cellsGroups = cellsGroups ?? new TableSupportCellGroup[0];
        }

        public TableSupportSource(TableSupportCellGroup cellsGroup)
        {
            _cellsGroups = new[] { cellsGroup };
        }

        public TableSupportSource()
        {
            _cellsGroups = new TableSupportCellGroup[0];
        }

        public void AppendTableSupportCellGroup(TableSupportCellGroup group)
        {
            _cellsGroups = _cellsGroups.Union(new[] { group }).ToArray();
        }

        public void PrependTableSupportCellGroup(TableSupportCellGroup group)
        {
            _cellsGroups = new[] { group }.Union(_cellsGroups).ToArray();
        }

        public event Action<NSIndexPath> Deleted;

        public NSIndexPath LastSelectedRow { get; set; }

        public UIImage Header { get; set; }
        public UIImage Footer { get; set; }

        public Func<UITableView, UIView> GetFooter { get; set; }

        public int FooterOffset { get; set; } = 1;

        public int FooterHeight { get; set; } = 0;

        public int HeaderHeight { get; set; }

        public bool CapitalizeGroupHeader { get; set; } = true;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            _tableView = tableView;
            var cell = _cellsGroups[indexPath.Section].Cells[indexPath.Row].GetCell(this, tableView, indexPath);

            if (AppDelegate.IsIpad)
            {
                cell.PreservesSuperviewLayoutMargins = false;
                cell.SeparatorInset = UIEdgeInsets.Zero;
                cell.LayoutMargins = UIEdgeInsets.Zero;
            }
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (_cellsGroups.Count() == 0)
                return 0;

            return (_cellsGroups[section].Cells ?? new TableSupportCell[0]).Length;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (section == 0 && Header != null)
            {
                var headerView = new UIView(new CGRect(0, 0, tableView.Bounds.Width, HeaderHeight + 60));
                var imageView = new UIImageView(Header);
                imageView.Frame = new CGRect(0, 0, headerView.Bounds.Width, HeaderHeight);
                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                imageView.Center = new CGPoint(headerView.Bounds.Width / 2, headerView.Bounds.Height / 2);
                headerView.AddSubview(imageView);
                return headerView;
            }

            return null;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            if (section == _cellsGroups.Length - FooterOffset)
            {
                if (Footer != null)
                {
                    var footerView = new UIView(new CGRect(0, 0, tableView.Bounds.Width, FooterHeight + 60));
                    var imageView = new UIImageView(Footer);
                    imageView.Frame = new CGRect(0, 0, footerView.Bounds.Width, FooterHeight);
                    imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    imageView.Center = new CGPoint(footerView.Bounds.Width / 2, footerView.Bounds.Height / 2);
                    footerView.AddSubview(imageView);
                    return footerView;
                }

                if (GetFooter != null)
                {
                    return GetFooter(tableView);
                }
            }

            return null;
        }

        public UITableViewCellEditingStyle EditingStyle = UITableViewCellEditingStyle.Delete;

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return EditingStyle;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            if (!_cellsGroups[section].Visible)
                return 0.01f;

            if (_cellsGroups.Count() == 0)
                return 0.01f;

            if (_cellsGroups[section].Cells.All(cell => cell.IsVisible == false))
                return 0.01f;

            return (Footer != null || GetFooter != null) && section == _cellsGroups.Length - FooterOffset ? FooterHeight : UITableView.AutomaticDimension;
        }

        public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
        {
            if (!string.IsNullOrEmpty(_cellsGroups[section].Header))
            {
                var header = (UITableViewHeaderFooterView)headerView;
                // Lower case header
                if (!CapitalizeGroupHeader)
                {
                    header.TextLabel.Text = _cellsGroups[section].Header;
                }
                // Modify the header's font size
                // header.TextLabel.Font = header.TextLabel.Font.WithSize(15f);
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return _cellsGroups[indexPath.Section].Cells[indexPath.Row].CanEdit;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                Deleted?.Invoke(indexPath);
            }
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (!_cellsGroups[section].Visible)
                return 0.01f;

            if (_cellsGroups.Count() == 0)
                return 0.01f;

            if (_cellsGroups[section].Cells.All(cell => cell.IsVisible == false))
                return 0.01f;

            return Header != null && section == 0 ? HeaderHeight + 60 : UITableView.AutomaticDimension;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _cellsGroups.Length;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if (!_cellsGroups[section].Visible)
                return "";

            if (_cellsGroups.Count() == 0)
                return "";

            if (_cellsGroups[section].Cells.All(cell => cell.IsVisible == false))
                return "";

            return _cellsGroups[section].Header;
        }

        public override bool ShouldHighlightRow(UITableView tableView, NSIndexPath rowIndexPath)
        {
            return _cellsGroups[rowIndexPath.Section].Cells[rowIndexPath.Row].ShouldHighlight;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            LastSelectedRow = indexPath;

            _cellsGroups[indexPath.Section].Cells[indexPath.Row].Selected(tableView, tableView.CellAt(indexPath));

            if (_cellsGroups[indexPath.Section].Cells[indexPath.Row].ShouldDeselectOnSelect)
            {
                tableView.DeselectRow(indexPath, true);
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (!_cellsGroups[indexPath.Section].Visible) return 0;

            if (_heights.ContainsKey(indexPath))
            {
                return _heights[indexPath];
            }

            var cell = _cellsGroups[indexPath.Section].Cells[indexPath.Row] as IProvidesHeight;

            return cell?.Height ?? UITableView.AutomaticDimension;
        }

        private Tuple<int, int> GetIndexOf(TableSupportCell cell)
        {
            for (var x = 0; x < _cellsGroups.Length; x++)
            {
                for (var y = 0; y < _cellsGroups[x].Cells.Length; y++)
                {
                    if (_cellsGroups[x].Cells[y] == cell)
                    {
                        return new Tuple<int, int>(x, y);
                    }
                }
            }

            return null;
        }

        public void UpdateHeight(NSIndexPath index, nfloat height)
        {
            var different = false;

            if (!_heights.ContainsKey(index))
            {
                _heights.Add(index, height);
                different = true;
            }
            else if (_heights[index] != height)
            {
                _heights[index] = height;
                different = true;
            }

            if (different)
            {
                _tableView.ReloadRows(new[] { index }, UITableViewRowAnimation.Automatic);
            }
        }
    }
}