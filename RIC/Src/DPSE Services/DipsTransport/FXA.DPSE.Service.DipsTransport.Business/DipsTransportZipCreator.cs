using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using System.IO;
using FXA.DPSE.Service.DipsTransport.Common.Helper;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public class DipsTransportZipCreator : IDipsTransportZipCreator
    {
        public FileInfo CreateZip(FileInfo target, IList<FileInfo> files, bool removeOriginalDirectoryPath = true)
        {
            target = SevenZipHelper.ZipFiles(files, target, removeOriginalDirectoryPath);
            return target;
        }

        public FileInfo CreateZip(FileInfo target, FileInfo file, bool removeOriginalDirectoryPath = true)
        {
            target = SevenZipHelper.ZipFiles(new List<FileInfo> { file }, target, removeOriginalDirectoryPath);
            return target;
        }
    }
}