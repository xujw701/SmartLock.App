using SmartLock.Model.BlueToothLe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface ITrackedBleService
    {
        List<KeyboxHistory> Records { get; }

        void Init();
        Task StartLock();
        Task StartUnlock();
        void SetKeyboxHistoryLocked(KeyboxHistory keyboxHistory);
    }
}
