using System;
using System.Collections.Generic;
using System.IO;
using FXA.DPSE.Service.DipsTransport.DataAccess;

namespace FXA.DPSE.Service.DipsTransport.Business.Core
{
    public interface IDipsTransportMetadataCreator
    {
        FileInfo GenerateMetadata(DateTime reportDate, IList<Transmission> transmissions, DirectoryInfo directory);
    }
}