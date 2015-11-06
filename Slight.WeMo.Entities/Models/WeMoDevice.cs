namespace Slight.WeMo.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class WeMoDevice
    {
        [Key]
        public string DeviceId { get; set; }

        [StringLength(128)]
        public string FriendlyName { get; set; }

        [StringLength(32)]
        public string DeviceType { get; set; }

        [StringLength(32)]
        public string ModelName { get; set; }

        [StringLength(32)]
        public string ModelNumber { get; set; }

        [StringLength(14)]
        public string SerialNumber { get; set; }

        [StringLength(12)]
        public string MacAddress { get; set; }

        [StringLength(32)]
        public string FirmwareVersion { get; set; }

        [StringLength(16)]
        public string Host { get; set; }

        public int Port { get; set; }

        [StringLength(36)]
        public string Location { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastDetected { get; set; }

        public bool Disabled { get; set; }
    }
}
