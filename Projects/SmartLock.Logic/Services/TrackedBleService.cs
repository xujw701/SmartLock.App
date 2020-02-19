using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Logic.Services
{
    public class TrackedBleService : ITrackedBleService
    {
        private const string StorageKey = "TrackedBleService";
        private const string KeyboxHistoryKey = "KeyboxHistory";

        private readonly IContainedStorage _containedStorage;
        private readonly IWebService _webService;
        private readonly IUserSession _userSession;
        private readonly IBlueToothLeService _blueToothLeService;

        private KeyboxHistories _keyboxHistories;
        private KeyboxHistory _currenHistory;

        public List<KeyboxHistory> Records => _keyboxHistories.Records.OrderByDescending(r => r.InTime).ToList();

        public TrackedBleService(IContainedStorage containedStorage, IWebService webService, IUserSession userSession, IBlueToothLeService blueToothLeService)
        {
            _containedStorage = containedStorage;
            _webService = webService;
            _userSession = userSession;
            _blueToothLeService = blueToothLeService;

            Init();
        }

        public void Init()
        {
            _containedStorage.Init(StorageKey);

            _blueToothLeService.OnLocked += BlueToothLeService_OnLocked;

            LoadObject();
        }

        public async Task StartLock()
        {
            await _blueToothLeService.StartSetLock(true);
        }

        public async Task StartUnlock()
        {
            await _blueToothLeService.StartSetLock(false);

            _currenHistory = new KeyboxHistory()
            {
                Id = Guid.NewGuid().ToString(),
                LockId = _blueToothLeService.ConnectedDevice.Id.ToString(),
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

        public async Task<bool> ValidateKeybox(BleDevice bleDevice)
        {
            var keyboxGetResponse = await _webService.GetKeybox(uuid: bleDevice.Id.ToString());
            return keyboxGetResponse != null;
        }

        private void BlueToothLeService_OnLocked()
        {
            if (_currenHistory != null)
            {
                SetKeyboxHistoryLocked(_currenHistory);

                _currenHistory = null;
            }
        }

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
    }
}
