using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Framework.Common.Security.SHA1
{
    public static class SHA1Crypto
    {
        public static string GetSHA1String(this string inputString)
        {
            var sb = new StringBuilder();

            foreach (var b in GetSHA1(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetSHA1(this string inputString)
        {
            var algorithm 
                = System.Security.Cryptography.SHA1.Create();
            
            return algorithm.ComputeHash(
                Encoding.UTF8.GetBytes(inputString)
            );
        }

        public static bool ValidateWithSHA1(this string inputString, string storedHashData)
        {
            return (
                string.Compare(
                    GetSHA1String(inputString), storedHashData, StringComparison.CurrentCultureIgnoreCase
                ) == 0
            );
        }
    }
}