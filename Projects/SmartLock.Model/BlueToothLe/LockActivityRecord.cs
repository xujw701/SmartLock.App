using System;

namespace SmartLock.Model.BlueToothLe
{
    public class LockActivityRecord
    {
        public string LockId { get; set; }
        public string LockName { get; set; }
        public LockActivity LockActivity { get; set; }
        public DateTime DateTime { get; set; }
    }

    public enum LockActivity
    {
        Lock = 0,
        UnLock = 1
    }
}
