using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FXA.DPSE.Framework.Common.Extensions
{
    public static class TypesExtensions
    {
        public static bool IsAbsoluteUrl(this string value)
        {
            Uri result;
            return Uri.TryCreate(value, UriKind.Absolute, out result);
        }

        public static byte[] GetBytes(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(this byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static bool IsValidJson(this string value)
        {
            try
            {
                var x = JToken.Parse(value);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}