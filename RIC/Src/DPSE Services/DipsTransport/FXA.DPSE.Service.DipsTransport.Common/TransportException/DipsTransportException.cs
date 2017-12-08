using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsTransport.Common.TransportException
{
    public enum DipsTransportErrorType
    {
        DirectoryAccessError,
        FileAccessError,
        DatabaseAccessError,
        EntityNotFoundError
    }
}