using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Model
{
    public class ResponseStatusCodeAttribute : Attribute
    {
        public int HttpCode { get; private set; }
        public string Description { get; private set; }

        internal ResponseStatusCodeAttribute(int httpCode, string description)
        {
            HttpCode = httpCode;
            Description = description;
        }
    }
}