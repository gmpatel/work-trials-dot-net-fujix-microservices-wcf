using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(FileInfo file)
        {
            return file.Exists;
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public FileInfo[] GetFilesFromDirectory(DirectoryInfo directory)
        {
            return directory.GetFiles();
        }

        public string GetSHA1String(FileInfo file)
        {
            return GetSHA1String(Convert.ToBase64String(File.ReadAllBytes(file.FullName)));
        }

        public byte[] GetSHA1(FileInfo file)
        {
            return GetSHA1(Convert.ToBase64String(File.ReadAllBytes(file.FullName)));
        }

        private static string GetSHA1String(string inputString)
        {
            var sb = new StringBuilder();

            foreach (var b in GetSHA1(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private static byte[] GetSHA1(string inputString)
        {
            var algorithm
                = System.Security.Cryptography.SHA1.Create();

            return algorithm.ComputeHash(
                Encoding.UTF8.GetBytes(inputString)
            );
        }

        private static bool ValidateWithSHA1(string inputString, string storedHashData)
        {
            return (
                string.Compare(
                    GetSHA1String(inputString), storedHashData, StringComparison.CurrentCultureIgnoreCase
                ) == 0
            );
        }
    }
}