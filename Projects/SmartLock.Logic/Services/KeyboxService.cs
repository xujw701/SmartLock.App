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
        private readonly IUserService _userService;
        private readonly ICacheManager _cacheManager;
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

        public KeyboxService(IWebService webService, IUserSession userSession, IUserService userService, ICacheManager cacheManager, ILocalBleService localBleService)
        {
            _webService = webService;
            _userSession = userSession;
            _userService = userService;
            _cacheManager = cacheManager;
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
                DateTime = DateTimeOffset.Now
            });

            if (allow)
            {
                await _localBleService.StartSetLock(false);

                await Task.Delay(10000);

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

        public async Task<List<Keybox>> GetKeyboxesIUnlocked()
        {
            var keyboxes = await _webService.GetKeyboxIUnlocked();

            if (keyboxes != null)
            {
                return keyboxes.Where(k => k.PropertyId.HasValue && k.UserId.HasValue && k.UserId.Value != _userSession.UserId).ToList();
            }

            return new List<Keybox>();
        }

        public async Task<Property> GetKeyboxProperty(int keyboxId, int propertyId, bool force = false)
        {
            var property = await _webService.GetKeyboxProperty(keyboxId, propertyId);

            if (property != null)
            {
                property.PropertyResource = await _webService.GetPropertyResources(keyboxId, propertyId);

                foreach (var resource in property.PropertyResource)
                {
                    resource.Image = await GetCachedPropertyResource(resource.ResPropertyId, force);
                }
            }

            return property;
        }

        public async Task UpdatePropertyResource(Property property)
        {
            if (property.ToUploadResource.Count > 0)
            {
                foreach (var resource in property.ToUploadResource)
                {
                    var data = _cacheManager.GetRawData(resource.NativePath);
                    await _webService.AddPropertyResource(0, property.PropertyId, data);
                }
            }

            if (property.PropertyResource.Count > 0)
            {
                foreach (var resource in property.PropertyResource)
                {
                    if (resource.ToDelete)
                    {
                        await _webService.DeletePropertyResource(0, property.PropertyId, resource.ResPropertyId);
                    }
                }
            }
        }

        public Cache SavePropertyResourceLocal(byte[] data)
        {
            _cacheManager.Init(CacheManager.PropertyLocalStorage);

            return _cacheManager.Save(data);
        }

        private async Task<Cache> GetCachedPropertyResource(int resPropertyId, bool force = false)
        {
            _cacheManager.Init(CacheManager.PropertyStorageKey);

            Cache cache = null;

            // Read from disk then
            var key = resPropertyId.ToString();
            cache = _cacheManager.Get(key);

            if (cache == null || force)
            {
                // Read from web api last
                var data = await _webService.GetPropertyResourceData(resPropertyId);
                cache = _cacheManager.Save(data, key);
            }

            return cache;
        }

        public async Task<List<KeyboxHistory>> GetKeyboxHistories(int keyboxId, int propertyId)
        {
            var keyboxHistories = await _webService.GetHistories(keyboxId, propertyId);

            foreach (var history in keyboxHistories)
            {
                if (history.ResPortraitId.HasValue)
                {
                    var portrait = await _userService.GetCachedPortrait(history.ResPortraitId.Value);

                    history.Portrait = portrait;
                }
            }

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

            property.PropertyId = result.Id;

            await UpdatePropertyResource(property);

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

            await UpdatePropertyResource(property);

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

        public async Task<List<PropertyFeedback>> GetAllPropertyFeedback()
        {
            var feedbacks = new List<PropertyFeedback>();

            var keyboxes = await _webService.GetMyKeybox();

            foreach (var keybox in keyboxes)
            {
                if (keybox.PropertyId.HasValue)
                {
                    var feedback = await _webService.GetPropertyFeedback(keybox.KeyboxId, keybox.PropertyId.Value);

                    foreach (var f in feedback)
                    {
                        if (f.ResPortraitId.HasValue)
                        {
                            var portrait = await _userService.GetCachedPortrait(f.ResPortraitId.Value);

                            f.Portrait = portrait;
                        }

                        f.KeyboxName = keybox.KeyboxName;
                    }

                    feedbacks.AddRange(feedback);
                }
            }

            return feedbacks.OrderByDescending(f => f.CreatedOn).ToList();
        }

        public async Task<List<PropertyFeedback>> GetPropertyFeedback(Keybox keybox, int propertyId)
        {
            var feedbacks = await _webService.GetPropertyFeedback(keybox.KeyboxId, propertyId);

            foreach (var feedback in feedbacks)
            {
                if (feedback.ResPortraitId.HasValue)
                {
                    var portrait = await _userService.GetCachedPortrait(feedback.ResPortraitId.Value);

                    feedback.Portrait = portrait;
                }

                feedback.KeyboxName = keybox.KeyboxName;
            }

            return feedbacks;
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
                        DateTime = DateTimeOffset.Now
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