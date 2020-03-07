using System;
using System.Collections.Generic;
using SmartLock.Model.Models;
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
        public event Action PlaceKeyboxClicked;
        public event Action<bool> TabClicked;
        public event Action Refresh;

        public KeyboxesView(KeyboxesController controller) : base(controller, "KeyboxesView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            BtnAddLock.TouchUpInside += (s, e) =>
            {
                PlaceKeyboxClicked?.Invoke();
            };

            BtnMine.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                TabClicked?.Invoke(true);
                UpdateUI(true);
            }));

            BtnOthers.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                TabClicked?.Invoke(false);
                UpdateUI(false);
            }));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Refresh?.Invoke();
        }

        public void Show(List<Keybox> keyboxes, bool placeLockButtonEnabled)
        {
            UpdatePlaceLockButton(placeLockButtonEnabled);

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

        public void UpdatePlaceLockButton(bool enabled)
        {
            BtnAddLock.BackgroundColor = enabled ? UIColor.FromRGB(13, 115, 244) : UIColor.FromRGB(230, 230, 230);
        }

        private void UpdateUI(bool mine)
        {
            IvMine.Hidden = !mine;
            IvOther.Hidden = mine;
        }
    }
}

