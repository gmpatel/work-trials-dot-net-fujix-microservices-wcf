using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Framework.Model.DipsTransport
{
    [DataContract]
    public class DipsTransportResponse
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}