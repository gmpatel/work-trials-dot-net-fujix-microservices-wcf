using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DTO.HealthMonitor
{
    [DataContract]
    public class HealthMonitorPostResponse
    {
        [DataMember]
        [Required]
        public Guid? Id { get; set; }
    }
}