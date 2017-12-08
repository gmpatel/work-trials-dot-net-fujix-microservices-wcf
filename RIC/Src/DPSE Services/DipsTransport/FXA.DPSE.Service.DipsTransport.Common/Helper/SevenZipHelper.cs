using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace FXA.DPSE.Service.DipsTransport.Common.Helper
{
    public static class SevenZipHelper
    {
        public static FileInfo ZipFiles(IList<FileInfo> sourceFiles, FileInfo targetFile, bool removeOriginalDirectoryPath)
        {
            using (var zipper = new ZipFile())
            {
                zipper.FlattenFoldersOnExtract = true;
                sourceFiles.Select(x => x.FullName).ToList().ForEach(x => zipper.AddFile(x, removeOriginalDirectoryPath ? string.Empty : null));
                zipper.Save(targetFile.FullName);
            }

            return targetFile;
        }
    }
}