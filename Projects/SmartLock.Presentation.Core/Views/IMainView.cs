using SmartLock.Presentation.Core.ViewControllers;
using System;

namespace SmartLock.Presentation.Core.Views
{
    public interface IMainView : IView
    {
        void SetTabs(HomeController homeController, KeyboxesController keyboxesController, ListingController listingController, NearbyController nearbyController, SettingController settingController);
    }
}
