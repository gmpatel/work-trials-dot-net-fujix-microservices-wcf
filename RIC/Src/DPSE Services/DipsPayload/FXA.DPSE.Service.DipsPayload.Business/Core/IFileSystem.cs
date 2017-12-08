using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsPayload.Business.Core
{
    public interface IFileSystem
    {
        bool FileExists(FileInfo file);
        bool FileExists(string filePath);
        bool DirectoryExists(DirectoryInfo directory);
        bool DirectoryExists(string directoryPath);
        FileInfo[] GetFilesFromDirectory(DirectoryInfo directory);
        string GetSHA1String(FileInfo file);
        byte[] GetSHA1(FileInfo file);
        DirectoryInfo CreateDirectory(DirectoryInfo directory);
        DirectoryInfo CreateDirectory(string directoryPath);
    }
}