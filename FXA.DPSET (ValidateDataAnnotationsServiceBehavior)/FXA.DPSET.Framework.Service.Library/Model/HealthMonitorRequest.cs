using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Library.Model
{
    [DataContract]
    public class HealthMonitorRequest
    {
        [DataMember]
        [Required]
        public Guid? Id { get; set; }
        
        [DataMember]
        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Message { get; set; }
    }
}