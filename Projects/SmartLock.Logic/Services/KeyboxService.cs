using SmartLock.Model.Ble;
using SmartLock.Model.Models;
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
        private const string StorageKey = "KeyboxService";
        private const string KeyboxHistoryKey = "KeyboxHistory";

        private readonly IContainedStorage _containedStorage;
        private readonly IWebService _webService;
        private readonly IUserSession _userSession;
        private readonly ILocalBleService _localBleService;

        private KeyboxHistories _keyboxHistories;
        private KeyboxHistory _currenHistory;

        private Keybox _connectedKeybox;

        private List<Keybox> _discoveredKeyboxes;

        public event Action<Keybox> OnKeyboxDiscovered;
        public event Action<Keybox> OnKeyboxConnected;
        public event Action OnLocked;
        public event Action OnUnlocked;

        public bool IsOn => _localBleService.IsOn;
        public List<Keybox> DiscoveredKeyboxes => _discoveredKeyboxes;
        public Keybox ConnectedKeybox => _connectedKeybox;
        public bool DeviceConnected => _localBleService.DeviceConnected;

        public List<KeyboxHistory> Records => _keyboxHistories.Records.OrderByDescending(r => r.InTime).ToList();

        public KeyboxService(IContainedStorage containedStorage, IWebService webService, IUserSession userSession, ILocalBleService localBleService)
        {
            _containedStorage = containedStorage;
            _webService = webService;
            _userSession = userSession;
            _localBleService = localBleService;

            Init();
        }

        public void Init()
        {
            _containedStorage.Init(StorageKey);

            _discoveredKeyboxes = new List<Keybox>();

            _localBleService.OnDeviceDiscovered += LocalBleService_OnDeviceDiscovered;
            _localBleService.OnDeviceConnected += LocalBleService_OnDeviceConnected;

            _localBleService.OnLocked += LocalBleService_OnLocked;
            _localBleService.OnUnlocked += LocalBleService_OnUnlocked;

            LoadObject();
        }

        public async Task StartScanningForKeyboxesAsync()
        {
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
        }

        public async Task StartLock()
        {
            await _localBleService.StartSetLock(true);
        }

        public async Task StartUnlock()
        {
            await _localBleService.StartSetLock(false);

            _currenHistory = new KeyboxHistory()
            {
                Id = Guid.NewGuid().ToString(),
                LockId = _localBleService.ConnectedDevice.Id.ToString(),
                Opener = "Wayne Leonard",
                InTime = DateTime.Now
            };

            _keyboxHistories.Records.Add(_currenHistory);

            SaveObject();
        }

        public void SetKeyboxHistoryLocked(KeyboxHistory keyboxHistory)
        {
            var record = _keyboxHistories.Records.FirstOrDefault(d => !string.IsNullOrEmpty(d.Id) && d.Id.Equals(keyboxHistory.Id));

            if (record != null)
            {
                record.OutTime = DateTime.Now;
            }

            SaveObject();
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

        private void LocalBleService_OnLocked()
        {
            if (_currenHistory != null)
            {
                SetKeyboxHistoryLocked(_currenHistory);

                _currenHistory = null;
            }

            OnLocked?.Invoke();
        }

        private void LocalBleService_OnUnlocked()
        {
            OnUnlocked?.Invoke();
        }

        #region ContainedStorage

        private void LoadObject()
        {
            _keyboxHistories = _containedStorage.GetSerializedObject<KeyboxHistories>(KeyboxHistoryKey);

            if (_keyboxHistories == null)
            {
                _keyboxHistories = new KeyboxHistories()
                {
                    Records = new List<KeyboxHistory>()
                };
            }
        }

        private void SaveObject()
        {
            _containedStorage.StoreObjectSerialized(KeyboxHistoryKey, _keyboxHistories);
        }

        private void DeleteObject()
        {
            _containedStorage.DeleteSerializedObject(KeyboxHistoryKey);
        }

        #endregion
    }
}