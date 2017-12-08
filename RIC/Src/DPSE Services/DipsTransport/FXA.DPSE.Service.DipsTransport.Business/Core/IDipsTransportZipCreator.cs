using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsTransport.Business.Core
{
    public interface IDipsTransportZipCreator
    {
        FileInfo CreateZip(FileInfo target, IList<FileInfo> files, bool removeOriginalDirectoryPaths = true);
        FileInfo CreateZip(FileInfo target, FileInfo file, bool removeOriginalDirectoryPaths = true);
    }
}