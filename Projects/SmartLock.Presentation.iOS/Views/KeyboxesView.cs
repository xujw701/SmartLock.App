using System;
using System.Collections.Generic;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public class KeyboxesView : TableView<IKeyboxesView>, IKeyboxesView
    {
        public event Action<Keybox> KeyboxClicked;

        public KeyboxesView(KeyboxesController controller) : base(controller)
        {
        }

        public void Show(List<Keybox> keyboxes)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

