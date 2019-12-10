using System;

namespace SmartLock.Model.BlueToothLe
{
    public class LockboxRecord
    {
        public string LockId { get; set; }
        public string LockName { get; set; }
        public LockActivity LockActivity { get; set; }
        public DateTime DateTime { get; set; }

        public string DateTimeString => DateTime.ToString("HH:mm:ss dd/MM/yyyy");
    }

    public enum LockActivity
    {
        Lock = 0,
        UnLock = 1
    }
}
