using SmartLock.Model.BlueToothLe;
using SmartLock.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Logic.Services
{
    public class TrackedBleService : ITrackedBleService
    {
        private const string StorageKey = "TrackedBleService";
        private const string LockActivityRecordKey = "LockActivityRecord";

        private readonly IBlueToothLeService _blueToothLeService;
        private readonly IContainedStorage _containedStorage;

        private LockboxRecords _lockActivityRecords;

        public List<LockboxRecord> Records => _lockActivityRecords.Records;

        public TrackedBleService(IContainedStorage containedStorage, IBlueToothLeService  blueToothLeService)
        {
            _containedStorage = containedStorage;
            _blueToothLeService = blueToothLeService;

            //_containedStorage.Init(StorageKey);

            //LoadObject();
        }

        public void Init()
        {
            _containedStorage.Init(StorageKey);

            LoadObject();
        }

        public void Lock()
        {
            _blueToothLeService.SetLock(true);
        }

        public void Unlock()
        {
            _blueToothLeService.SetLock(false);

            var record = new LockboxRecord()
            {
                LockId = _blueToothLeService.ConnectedDevice.Id.ToString(),
                LockName = _blueToothLeService.ConnectedDevice.Name,
                LockActivity = LockActivity.UnLock,
                DateTime = DateTime.Now
            };

            _lockActivityRecords.Records.Add(record);

            SaveObject();
        }

        private void LoadObject()
        {
            _lockActivityRecords = _containedStorage.GetSerializedObject<LockboxRecords>(LockActivityRecordKey);

            if (_lockActivityRecords == null)
            {
                _lockActivityRecords = new LockboxRecords()
                {
                    Records = new List<LockboxRecord>()
                };
            }
        }

        private void SaveObject()
        {
            _containedStorage.StoreObjectSerialized(LockActivityRecordKey, _lockActivityRecords);
        }

        private void DeleteObject()
        {
            _containedStorage.DeleteSerializedObject(LockActivityRecordKey);
        }
    }
}
