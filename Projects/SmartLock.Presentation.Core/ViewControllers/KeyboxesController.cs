using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Collections.Generic;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxesController : ViewController<IKeyboxesView>
    {
        public KeyboxesController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.KeyboxClicked += (keybox) => Push<KeyboxDetailController>(vc => vc.Keybox = keybox);
        }

        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            View.Show(LoadData());
        }

        private List<Keybox> LoadData()
        {
            var data = new List<Keybox>()
            {
                new Keybox()
                {
                    Name = "28e Oak View Terrace",
                    Address = "Albany, Auckland",
                    BatteryLevel = 100
                },
                new Keybox()
                {
                    Name = "1/6 Alberon Street",
                    Address = "Parnell, Auckland",
                    BatteryLevel = 80
                },
                new Keybox()
                {
                    Name = "2/6 Alberon Street",
                    Address = "Parnell, Auckland",
                    BatteryLevel = 70
                },
                new Keybox()
                {
                    Name = "14 Athfield Drive",
                    Address = "Devonport, Auckland",
                    BatteryLevel = 45
                },
                new Keybox()
                {
                    Name = "16 Athfield Drive",
                    Address = "Devonport, Auckland",
                    BatteryLevel = 42
                },
                new Keybox()
                {
                    Name = "12 Arawai Terrace",
                    Address = "Papakura, Auckland",
                    BatteryLevel = 26
                },
                new Keybox()
                {
                    Name = "52 Delamore Drive",
                    Address = "Oneroa, Auckland",
                    BatteryLevel = 9
                },
            };

            return data;
        }
    }
}
