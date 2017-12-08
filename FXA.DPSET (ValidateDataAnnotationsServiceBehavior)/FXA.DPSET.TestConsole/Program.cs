using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Common.PGP;

namespace FXA.DPSET.TestConsole
{
    class Program
    {
        public static void Main()
        {
            //var info = new Dictionary<string, string> { { "email", "gmpatel@live.com" }, { "mobile", "0414854093" } };

            //var x = PgpKeyGenerator.GenerateKeys("Gunjan Patel", info,  "abcd1234", new DirectoryInfo(@"C:\Temp"));

            PGPCrypto.Encrypt(new FileInfo(@"C:\Temp\Notepad.exe"));
            PGPCrypto.Encrypt(new FileInfo(@"C:\Temp\AnnualLeaveFramework.log"));
            PGPCrypto.Encrypt(new FileInfo(@"C:\Temp\Notepad.zip"));

            PGPCrypto.Decrypt(new FileInfo(@"C:\Temp\Notepad.exe.pgp"));
            PGPCrypto.Decrypt(new FileInfo(@"C:\Temp\AnnualLeaveFramework.log.pgp"));
            PGPCrypto.Decrypt(new FileInfo(@"C:\Temp\Notepad.zip.pgp"));

            Console.ReadKey();
        }
    }
}