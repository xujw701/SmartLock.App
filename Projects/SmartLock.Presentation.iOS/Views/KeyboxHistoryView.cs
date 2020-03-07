using System;
using System.Collections.Generic;
using System.Linq;
using SmartLock.Model.Models;
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

        private List<KeyboxHistory> _keyboxHistories;

        private int _filter;

        public event Action BackClick;

        public KeyboxHistoryView(KeyboxHistoryController controller) : base(controller, "KeyboxHistoryView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Btn7Days.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                Show(0);
            }));

            Btn30Days.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                Show(1);
            }));

            BtnAll.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                Show(2);
            }));

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));
        }

        public void Show(List<KeyboxHistory> keyboxHistories)
        {
            _keyboxHistories = keyboxHistories;

            Show(0);
        }

        private List<KeyboxHistory> GetFilteredHistories()
        {
            if (_filter == 0)
            {
                return _keyboxHistories.Where(k => (DateTimeOffset.Now - k.InOn.LocalDateTime).TotalDays <= 7).ToList();
            }
            else if (_filter == 1)
            {
                return _keyboxHistories.Where(k => (DateTimeOffset.Now - k.InOn.LocalDateTime).TotalDays <= 30).ToList();
            }

            return _keyboxHistories;
        }

        private void Show(int filter)
        {
            _filter = filter;

            if (_keyboxHistorySource == null)
            {
                _keyboxHistorySource = new KeyboxHistorySource(GetFilteredHistories());

                HistoryTableView.EstimatedRowHeight = 118f;
                HistoryTableView.RowHeight = UITableView.AutomaticDimension;
                HistoryTableView.Source = _keyboxHistorySource;
            }
            else
            {
                _keyboxHistorySource.KeyboxHistories = GetFilteredHistories();
            }

            HistoryTableView.ReloadData();

            Dot7Days.Hidden = filter != 0;
            Dot30Days.Hidden = filter != 1;
            DotAll.Hidden = filter != 2;
        }
    }
}

