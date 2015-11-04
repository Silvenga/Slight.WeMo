namespace Slight.WeMo.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Slight.WeMo.Entities.Enums;

    public class SwitchEvent
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(36)]
        public string DeviceId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        public SwitchState OldState { get; set; }

        public SwitchState CurrentState { get; set; }
    }
}
