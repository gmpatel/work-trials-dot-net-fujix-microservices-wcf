using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FXA.DPSE.Service.DTO.HealthMonitor
{
    [DataContract]
    public class HealthMonitorPostRequest
    {
        [DataMember]
        [Required]
        public Guid? Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Message { get; set; }
    }
}