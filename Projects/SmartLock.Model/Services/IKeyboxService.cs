using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface IKeyboxService
    {
        event Action<Keybox> OnKeyboxDiscovered;
        event Action<Keybox> OnKeyboxConnected;
        event Action OnLocked;
        event Action OnUnlocked;

        bool IsOn { get; }
        Keybox ConnectedKeybox { get; }

        List<Keybox> DiscoveredKeyboxes { get; }

        void Init();
        Task StartScanningForKeyboxesAsync();
        Task StopScanningForKeyboxesAsync();
        Task ConnectToKeyboxAsync(Keybox keybox);
        Task DisconnectKeyboxAsync(Keybox keybox);
        Task StartLock();
        Task StartUnlock();

        Task<List<Keybox>> GetMyListingKeyboxes();
        Task<Property> GetKeyboxProperty(int keyboxId, int propertyId);
        Task<List<KeyboxHistory>> GetKeyboxHistories(int keyboxId, int propertyId);
    }
}
