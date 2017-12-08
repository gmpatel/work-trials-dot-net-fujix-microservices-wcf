using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using System.IO;
using FXA.DPSE.Framework.Common.Security.PGP;
using FXA.DPSE.Service.DipsTransport.Common.Helper;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public class DipsTransportPgpCreator : IDipsTransportPgpCreator
    {
        public FileInfo CreatePgp(FileInfo file, bool removeOriginalFile = false)
        {
            return PGPCrypto.Encrypt(file, removeOriginalFile);
        }
    }
}