using System;

namespace SmartLock.Model.BlueToothLe
{
    public class KeyboxHistory
    {
        public string Id { get; set; }
        public string LockId { get; set; }
        public string Opener { get; set; }
        public DateTime InTime { get; set; }
        public DateTime? OutTime { get; set; }

        public string InTimeString => InTime.ToString("dd/MM/yy HH:mm");
        public string OutTimeString => OutTime == null ? "Still unlocked" : OutTime.Value.ToString("dd/MM/yy HH:mm");

        public string Duration
        {
            get
            {
                var diff = (OutTime == null ? DateTime.Now : OutTime.Value) - InTime;
                return $"{((int)diff.TotalMinutes).ToString()} mins";
            }
        }
    }
}
