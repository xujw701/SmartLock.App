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
    public partial class KeyboxesView : View<IKeyboxesView>, IKeyboxesView
    {
        private KeyboxSource _keyboxSource;

        public event Action<Keybox> KeyboxClicked;

        public KeyboxesView(KeyboxesController controller) : base(controller, "KeyboxesView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void Show(List<Keybox> keyboxes)
        {
            if (_keyboxSource == null)
            {
                _keyboxSource = new KeyboxSource(keyboxes, KeyboxClicked);

                KeyboxesTableView.EstimatedRowHeight = 190f;
                KeyboxesTableView.RowHeight = UITableView.AutomaticDimension;
                KeyboxesTableView.Source = _keyboxSource;
            }
            else
            {
                _keyboxSource.Keyboxes = keyboxes;
            }

            KeyboxesTableView.ReloadData();
        }
    }
}

