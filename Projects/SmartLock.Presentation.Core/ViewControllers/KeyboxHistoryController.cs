using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxHistoryController : ViewController<IKeyboxHistoryView>
    {
        private readonly ITrackedBleService _trackedBleService;
        private readonly IMessageBoxService _messageBoxService;

        public KeyboxHistoryController(IViewService viewService, ITrackedBleService trackedBleService, IMessageBoxService messageBoxService) : base(viewService)
        {
            _trackedBleService = trackedBleService;
            _messageBoxService = messageBoxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.KeyboxSetOut += View_KeyboxSetOut;
            View.Show(LoadData());
        }

        private async void View_KeyboxSetOut(KeyboxHistory keyboxHistory)
        {
            var ok = await _messageBoxService.ShowOkCancelAsync("Confirm", "Do you want to set it locked manually in the history?");

            if (ok)
            {
                _trackedBleService.SetKeyboxHistoryLocked(keyboxHistory);

                View.Show(LoadData());
            }
        }

        private List<KeyboxHistory> LoadData()
        {
            var result = _trackedBleService.Records;

            var demoData = new List<KeyboxHistory>()
            {
                new KeyboxHistory()
                {
                    Opener = "Mollie Kelley",
                    InTime = new DateTime(2019, 12, 9, 10, 5, 0),
                    OutTime = new DateTime(2019, 12, 9, 10, 30, 0),
                },
                new KeyboxHistory()
                {
                    Opener = "Winifred Terry",
                    InTime = new DateTime(2019, 12, 8, 13, 30, 0),
                    OutTime = new DateTime(2019, 12, 8, 13, 58, 0),
                },
                new KeyboxHistory()
                {
                    Opener = "Harry Lawrence",
                    InTime = new DateTime(2019, 12, 6, 14, 20, 0),
                    OutTime = new DateTime(2019, 12, 6, 14, 26, 0),
                },
                new KeyboxHistory()
                {
                    Opener = "Della Mills",
                    InTime = new DateTime(2019, 12, 5, 13, 2, 0),
                    OutTime = new DateTime(2019, 12, 5, 13, 22, 0),
                },
            };

            if (!result.Any(k => k.Opener.Equals("Della Mills")))
            {
                result.AddRange(demoData);
            }

            return result;
        }
    }
}
