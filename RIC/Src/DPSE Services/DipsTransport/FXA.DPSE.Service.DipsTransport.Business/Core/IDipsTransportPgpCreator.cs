using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsTransport.Business.Core
{
    public interface IDipsTransportPgpCreator
    {
        FileInfo CreatePgp(FileInfo file, bool removeOriginalFile = false);
    }
}