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
    public partial class KeyboxHistoryView : View<IKeyboxHistoryView>, IKeyboxHistoryView
    {
        private KeyboxHistorySource _keyboxHistorySource;
        public event Action BackClick;
        public event Action<KeyboxHistory> KeyboxSetOut;

        public KeyboxHistoryView(KeyboxHistoryController controller) : base(controller, "KeyboxHistoryView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));
        }

        public void Show(List<KeyboxHistory> keyboxHistories)
        {
            if (_keyboxHistorySource == null)
            {
                _keyboxHistorySource = new KeyboxHistorySource(keyboxHistories);

                HistoryTableView.EstimatedRowHeight = 118f;
                HistoryTableView.RowHeight = UITableView.AutomaticDimension;
                HistoryTableView.Source = _keyboxHistorySource;
            }
            else
            {
                _keyboxHistorySource.KeyboxHistories = keyboxHistories;
            }

            HistoryTableView.ReloadData();
        }
    }
}

