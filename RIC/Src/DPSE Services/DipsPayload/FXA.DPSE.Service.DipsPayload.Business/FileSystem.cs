using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsPayload.Business.Core;

namespace FXA.DPSE.Service.DipsPayload.Business
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

        public bool DirectoryExists(DirectoryInfo directory)
        {
            return directory.Exists;
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
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

        public DirectoryInfo CreateDirectory(DirectoryInfo directory)
        {
            if (!Directory.Exists(directory.FullName))
                Directory.CreateDirectory(directory.FullName);

            return directory;
        }

        public DirectoryInfo CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return new DirectoryInfo(directoryPath);
        }
    }
}