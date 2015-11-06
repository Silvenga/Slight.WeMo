namespace Slight.WeMo.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Slight.WeMo.Entities.Enums;

    public class WeMoDeviceState
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(36)]
        public WeMoDevice Device { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        public BinaryState CurrentState { get; set; }
    }
}
