using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FXA.DPSE.Framework.Web.Service.Library.Processors
{
    public interface IDipsTrasportProcessor
    {
        bool Process(Guid clientProcessId);
    }
}