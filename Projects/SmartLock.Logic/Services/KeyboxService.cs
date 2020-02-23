using SmartLock.Model.Ble;
using SmartLock.Model.Models;
using SmartLock.Model.Request;
using SmartLock.Model.Server;
using SmartLock.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Logic.Services
{
    public class KeyboxService : IKeyboxService
    {
        private readonly IWebService _webService;
        private readonly IUserSession _userSession;
        private readonly ILocalBleService _localBleService;

        private Keybox _connectedKeybox;

        private List<Keybox> _discoveredKeyboxes;

        public event Action<bool> OnBleStateChanged;
        public event Action<Keybox> OnKeyboxDiscovered;
        public event Action<Keybox> OnKeyboxConnected;
        public event Action OnKeyboxDisconnected;

        public event Action OnLocked;
        public event Action OnUnlocked;

        public bool IsOn => _localBleService.IsOn;
        public List<Keybox> DiscoveredKeyboxes => _discoveredKeyboxes;
        public Keybox ConnectedKeybox => _connectedKeybox;
        public bool DeviceConnected => _localBleService.DeviceConnected;

        public KeyboxService(IWebService webService, IUserSession userSession, ILocalBleService localBleService)
        {
            _webService = webService;
            _userSession = userSession;
            _localBleService = localBleService;

            Init();
        }

        public void Init()
        {
            _discoveredKeyboxes = new List<Keybox>();

            _localBleService.OnBleStateChanged += LocalBleService_OnBleStateChanged;
            _localBleService.OnDeviceDiscovered += LocalBleService_OnDeviceDiscovered;
            _localBleService.OnDeviceConnected += LocalBleService_OnDeviceConnected;
            _localBleService.OnDeviceDisconnected += LocalBleService_OnDeviceDisconnected;

            _localBleService.OnLocked += LocalBleService_OnLocked;
            _localBleService.OnUnlocked += LocalBleService_OnUnlocked;
        }

        public async Task StartScanningForKeyboxesAsync()
        {
            Clear();

            await _localBleService.StartScanningForDevicesAsync();
        }

        public async Task StopScanningForKeyboxesAsync()
        {
            await _localBleService.StopScanningForDevicesAsync();
        }

        public async Task ConnectToKeyboxAsync(Keybox keybox)
        {
            keybox.State = DeviceState.Connecting;
            await _localBleService.ConnectToDeviceAsync(keybox.Uuid);
        }

        public async Task DisconnectKeyboxAsync(Keybox keybox)
        {
            await _localBleService.DisconnectDeviceAsync(keybox.Uuid);
            keybox.State = DeviceState.Disconnected;

            Clear();
        }

        public async Task StartLock()
        {
            await _localBleService.StartSetLock(true);
        }

        public async Task<bool> StartUnlock()
        {
            var allow = await _webService.Unlock(_connectedKeybox.KeyboxId, new KeyboxHistoryPostDto()
            {
                DateTime = DateTime.Now
            });

            if (allow)
            {
                await _localBleService.StartSetLock(false);

                await Task.Delay(5000);

                await _localBleService.StartSetLock(true);

                _userSession.SaveKeyboxStatus(true);
            }

            return allow;
        }

        public async Task<List<Keybox>> GetMyListingKeyboxes()
        {
            var keyboxes = await _webService.GetMyKeybox();

            if (keyboxes != null)
            {
                return keyboxes.Where(k => k.PropertyId.HasValue).ToList();
            }

            return new List<Keybox>();
        }

        public async Task<Property> GetKeyboxProperty(int keyboxId, int propertyId)
        {
            var property = await _webService.GetKeyboxProperty(keyboxId, propertyId);

            return property;
        }

        public async Task<List<KeyboxHistory>> GetKeyboxHistories(int keyboxId, int propertyId)
        {
            var keyboxHistories = await _webService.GetHistories(keyboxId, propertyId);

            return keyboxHistories.OrderByDescending(h => h.InOn).ToList();
        }

        public async Task<bool> PlaceLock(Keybox keybox, Property property)
        {
            var result = await _webService.CreateKeyboxProperty(keybox.KeyboxId, new KeyboxPropertyPostPutDto()
            {
                CompanyId = keybox.CompanyId,
                BranchId = keybox.BranchId,
                KeyboxName = property.PropertyName,
                PropertyName = property.PropertyName,
                Address = property.Address,
                Notes = property.Notes,
                Price = property.Price,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                FloorArea = property.FloorArea,
                LandArea = property.LandArea
            });

            if (result != null)
            {
                return result.Id > 0;
            }

            return false;
        }

        public async Task<bool> PlaceLockUpdate(Keybox keybox, Property property)
        {
            await _webService.UpdateKeyboxProperty(keybox.KeyboxId, property.PropertyId, new KeyboxPropertyPostPutDto()
            {
                CompanyId = keybox.CompanyId,
                BranchId = keybox.BranchId,
                KeyboxName = property.PropertyName,
                PropertyName = property.PropertyName,
                Address = property.Address,
                Notes = property.Notes,
                Price = property.Price,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                FloorArea = property.FloorArea,
                LandArea = property.LandArea
            });

            return true;
        }

        public async Task EndKeyboxProperty(int keyboxId, int propertyId)
        {
            await _webService.EndKeyboxProperty(keyboxId, propertyId);
        }

        public async Task CreatePropertyFeedback(int keyboxId, int propertyId, string content)
        {
            await _webService.CreatePropertyFeedback(keyboxId, propertyId, new FeedbackPostDto()
            {
                Content = content
            });
        }

        public async Task<List<PropertyFeedback>> GetPropertyFeedback(int keyboxId, int propertyId)
        {
            return await _webService.GetPropertyFeedback(keyboxId, propertyId);
        }

        private void LocalBleService_OnBleStateChanged(bool isOn)
        {
            OnBleStateChanged?.Invoke(isOn);
        }

        private void LocalBleService_OnDeviceDiscovered(BleDevice bleDevice)
        {
            Task.Run(async () =>
            {
                var existedKebox = _discoveredKeyboxes.FirstOrDefault(k => k.KeyboxId.Equals(bleDevice.Id.ToString()));
                if (existedKebox != null) return;

                var keybox = await _webService.GetKeybox(uuid: bleDevice.Id.ToString());

                if (keybox != null && !_discoveredKeyboxes.Exists(d => d.Uuid.ToString().Equals(keybox.Uuid)))
                {
                    keybox.State = bleDevice.State;

                    _discoveredKeyboxes.Add(keybox);

                    OnKeyboxDiscovered?.Invoke(keybox);
                }
            });
        }

        private void LocalBleService_OnDeviceConnected(BleDevice bleDevice)
        {
            var keybox = _discoveredKeyboxes.FirstOrDefault(k => k.Uuid.Equals(bleDevice.Id.ToString()));

            if (keybox != null)
            {
                keybox.KeyboxName = bleDevice.Name;
                keybox.BatteryLevel = bleDevice.BatteryLevel;
                keybox.State = bleDevice.State;

                _connectedKeybox = keybox;

                OnKeyboxConnected?.Invoke(keybox);
            }
        }

        private void LocalBleService_OnDeviceDisconnected()
        {
            OnKeyboxDisconnected?.Invoke();
        }

        private void LocalBleService_OnLocked()
        {
            OnLocked?.Invoke();
        }

        private void LocalBleService_OnUnlocked()
        {
            if (_userSession.KeyboxStatus)
            {
                Task.Run(async () =>
                {
                    var allow = await _webService.Lock(_connectedKeybox.KeyboxId, new KeyboxHistoryPostDto()
                    {
                        DateTime = DateTime.Now
                    });
                });
            }

            OnUnlocked?.Invoke();
        }

        private void Clear()
        {
            // Clear the previous results
            _discoveredKeyboxes = new List<Keybox>();

            _connectedKeybox = null;
        }
    }
}