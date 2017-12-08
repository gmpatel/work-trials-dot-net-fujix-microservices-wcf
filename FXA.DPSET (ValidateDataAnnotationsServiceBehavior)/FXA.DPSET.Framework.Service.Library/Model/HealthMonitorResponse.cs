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
    public class HealthMonitorResponse
    {
        [DataMember]
        [Required]
        public Guid? Id { get; set; }
    }
}