using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.TableSupport
{
    public class TableSupportCellGroup
    {
        public string Header { get; set; }

        public TableSupportCell[] Cells { get; set; }

        public bool Visible { get; set; } = true;

        public TableSupportCellGroup()
        {
        }

        public TableSupportCellGroup(string header, TableSupportCell[] cells)
        {
            Header = header;
            Cells = cells;
        }

        public TableSupportCellGroup(TableSupportCell[] cells)
        {
            Cells = cells;
        }

        public TableSupportCellGroup(string header, TableSupportCell cell)
        {
            Header = header;
            Cells = new[] { cell };
        }

        public TableSupportCellGroup(TableSupportCell cell)
        {
            Cells = new[] { cell };
        }
    }
}