using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Framework.Common.Security.Base64
{
    public static class Base64Coversion
    {
        public static string Base64Decode(string data)
        {
            var toDecodeByte = Convert.FromBase64String(data);

            var encoder = new UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            var charCount = utf8Decode.GetCharCount(toDecodeByte, 0, toDecodeByte.Length);

            var decodedChar = new char[charCount];
            utf8Decode.GetChars(toDecodeByte, 0, toDecodeByte.Length, decodedChar, 0);
            
            var result = new string(decodedChar);
            return result;
        }
    }
}