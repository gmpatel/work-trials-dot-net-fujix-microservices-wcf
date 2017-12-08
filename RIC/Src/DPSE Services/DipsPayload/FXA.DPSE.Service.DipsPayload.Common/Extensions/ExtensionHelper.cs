using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.DipsPayload.Common.Extensions
{
    public static class ExtensionHelper
    {
        public static string GetLast(this string source, int tail)
        {
            if (tail >= source.Length)
                return source;

            return source.Substring(source.Length - tail);
        }
    }
}