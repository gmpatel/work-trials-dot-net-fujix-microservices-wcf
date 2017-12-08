using System;
using System.Collections.Generic;
using System.IO;
using Chilkat;

namespace FXA.DPSE.Service.DipsTransport.Common.Helper
{
    public static class ZipHelper
    {
        public static Zip GetZipper()
        {
            var zip = new Zip();
            var success = false;

            success = zip.UnlockComponent("FUJXRXSSH_sPfSFYMd3Cze");

            if (success != true)
            {
                throw new InvalidOperationException(zip.LastErrorText);
            }

            return zip;
        }

        public static FileInfo ZipFiles(IList<FileInfo> sourceFiles, FileInfo targetFile)
        {
            var zip = GetZipper();
            var success = zip.NewZip(targetFile.FullName);

            if (success != true)
            {
                throw new InvalidOperationException(zip.LastErrorText);
            }

            foreach (var file in sourceFiles)
            {
                success = zip.AppendOneFileOrDir(file.FullName.Replace(@"\", @"/"), false);

                if (success != true)
                {
                    throw new InvalidOperationException(zip.LastErrorText);
                }   
            }

            success = zip.WriteZipAndClose();

            if (success != true)
            {
                throw new InvalidOperationException(zip.LastErrorText);
            }

            while (!File.Exists(targetFile.FullName))
            {
            }

            return targetFile;
        }

        public static FileInfo ZipDirectory(DirectoryInfo sourceDirectory, FileInfo targetFile)
        {
            var zip = GetZipper();
            var success = zip.NewZip(targetFile.FullName);

            if (success != true)
            {
                throw new InvalidOperationException(zip.LastErrorText);
            }

            success = zip.AppendFiles(string.Format("{0}/{1}", sourceDirectory.FullName.Replace(@"\", @"/"), "*") , true);

            if (success != true)    
            {
                throw new InvalidOperationException(zip.LastErrorText);
            }

            success = zip.WriteZipAndClose();

            if (success != true)
            {
                throw new InvalidOperationException(zip.LastErrorText);
            }

            while (!File.Exists(targetFile.FullName))
            {
            }

            return targetFile;
        }
    }
}