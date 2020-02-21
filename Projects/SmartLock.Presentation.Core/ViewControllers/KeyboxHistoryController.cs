using SmartLock.Model.Models;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class KeyboxHistoryController : ViewController<IKeyboxHistoryView>
    {
        private readonly IKeyboxService _keyboxService;
        private readonly IMessageBoxService _messageBoxService;

        private List<KeyboxHistory> _keyboxHistories;

        public Keybox Keybox;
        public Property Property;

        public KeyboxHistoryController(IViewService viewService, IMessageBoxService messageBoxService, IKeyboxService keyboxService) : base(viewService)
        {
            _messageBoxService = messageBoxService;
            _keyboxService = keyboxService;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
        }
        protected override void OnViewWillShow()
        {
            base.OnViewWillShow();

            DoSafeAsync(LoadData);
        }

        private async Task LoadData()
        {
            _keyboxHistories = await _keyboxService.GetKeyboxHistories(Keybox.KeyboxId, Property.PropertyId);

            View.Show(_keyboxHistories);
        }
    }
}
