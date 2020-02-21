using System;

namespace SmartLock.Model.Models
{
    public class KeyboxHistory
    {
        public int KeyboxHistoryId { get; set; }
        public int KeyboxId { get; set; }
        public int UserId { get; set; }
        public int? TmpUserId { get; set; }
        public int PropertyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime InOn { get; set; }
        public DateTime? OutOn { get; set; }

        public string Name => $"{FirstName} {LastName}";
        public string InOnString => InOn.ToString("dd/MM/yy HH:mm");
        public string OutOnString => OutOn == null ? "Still unlocked" : OutOn.Value.ToString("dd/MM/yy HH:mm");

        public string Duration
        {
            get
            {
                if (OutOn == null)
                {
                    return string.Empty;
                }
                else
                {
                    var diff = OutOn.Value - InOn;
                    return $"{((int)diff.TotalMinutes).ToString()} mins";
                }
            }
        }
    }
}
