using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface ITrackedBleService
    {
        List<LockboxRecord> Records { get; }

        void Init();
        void Lock();
        void Unlock();
    }
}
