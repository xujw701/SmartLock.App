using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartLock.Logic.Services
{
    public class TrackedBleService : ITrackedBleService
    {
        private const string StorageKey = "TrackedBleService";
        private const string KeyboxHistoryKey = "KeyboxHistory";

        private readonly IBlueToothLeService _blueToothLeService;
        private readonly IContainedStorage _containedStorage;

        private KeyboxHistories _keyboxHistories;

        public List<KeyboxHistory> Records => _keyboxHistories.Records;

        public TrackedBleService(IContainedStorage containedStorage, IBlueToothLeService  blueToothLeService)
        {
            _containedStorage = containedStorage;
            _blueToothLeService = blueToothLeService;

            Init();
        }

        public void Init()
        {
            _containedStorage.Init(StorageKey);

            _blueToothLeService.OnLocked += BlueToothLeService_OnLocked;

            LoadObject();
        }

        public void StartLock()
        {
            _blueToothLeService.StartSetLock(true);
        }

        public void StartUnlock()
        {
            _blueToothLeService.StartSetLock(false);

            var record = new KeyboxHistory()
            {
                Id = Guid.NewGuid().ToString(),
                LockId = _blueToothLeService.ConnectedDevice.Id.ToString(),
                Opener = "Wayne Leonard",
                InTime = DateTime.Now
            };

            _keyboxHistories.Records.Add(record);

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

        private void BlueToothLeService_OnLocked()
        {
            var bleDevice = _blueToothLeService.ConnectedDevice;

            if (bleDevice != null)
            {
                var record = _keyboxHistories.Records.FirstOrDefault(d => d.LockId.Equals(bleDevice.Id.ToString()) && d.OutTime == null);

                if (record != null)
                {
                    record.OutTime = DateTime.Now;
                }
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
