using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface IKeyboxService
    {
        event Action<bool> OnBleStateChanged;
        event Action<Keybox> OnKeyboxDiscovered;
        event Action<Keybox> OnKeyboxConnected;
        event Action OnKeyboxDisconnected;
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
        void DismssKeybox(Keybox keybox);
        Task StartLock();
        Task<bool> StartUnlock();

        Task<List<Keybox>> GetMyListingKeyboxes();
        Task<List<Keybox>> GetKeyboxesIUnlocked();
        Task<Property> GetKeyboxProperty(int keyboxId, int propertyId, bool force = false);
        Task UpdatePropertyResource(Property property);
        Cache SavePropertyResourceLocal(byte[] data);
        Task<List<KeyboxHistory>> GetKeyboxHistories(int keyboxId, int propertyId);
        Task<bool> PlaceLock(Keybox keybox, Property property);
        Task<bool> PlaceLockUpdate(Keybox keybox, Property property);
        Task EndKeyboxProperty(int keyboxId, int propertyId);
        Task CreatePropertyFeedback(int keyboxId, int propertyId, string content);
        Task<List<PropertyFeedback>> GetPropertyFeedback(Keybox keybox, int propertyId);
        Task<List<PropertyFeedback>> GetAllPropertyFeedback();
        void Clear();
    }
}
