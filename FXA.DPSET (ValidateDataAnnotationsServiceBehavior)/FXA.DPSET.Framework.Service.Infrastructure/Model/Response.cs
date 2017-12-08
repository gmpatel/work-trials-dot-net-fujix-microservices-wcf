using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Model
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public ResponseStatusCode Code { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}