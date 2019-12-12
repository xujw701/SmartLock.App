﻿using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class MainController : ViewController<IMainView>
    {
        private readonly HomeController _homeController;
        private readonly KeyboxesController _keyboxesController;
        private readonly ListingController _listingController;
        private readonly NearbyController _nearbyController;
        private readonly SettingController _settingController;

        public MainController(IViewService viewService, IBlueToothLeService blueToothLeService, ITrackedBleService trackedBleService) : base(viewService)
        {
            _homeController = new HomeController(viewService, blueToothLeService, trackedBleService);
            _keyboxesController = new KeyboxesController(viewService);
            _listingController = new ListingController(viewService, trackedBleService);
            _nearbyController = new NearbyController(viewService, blueToothLeService);
            _settingController = new SettingController(viewService);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.SetTabs(_homeController, _keyboxesController, _listingController, _nearbyController, _settingController);
        }
    }
}
