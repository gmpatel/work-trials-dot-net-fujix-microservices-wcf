using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Errors;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Ping;
using FXA.DPSET.Framework.Service.Infrastructure.Attributes.Validators;
using FXA.DPSET.Framework.Service.Infrastructure.Exceptions;
using FXA.DPSET.Framework.Service.Library.Model;
using log4net;
using log4net.Core;

namespace FXA.DPSET.Framework.Service.Library
{
    [ErrorBehavior]
    [ValidateDataAnnotationsServiceBehavior]
    public class HealthMonitorService : IHealthMonitorService
    {
        public HealthMonitorResponse Post(HealthMonitorRequest request)
        {
            Thread.Sleep(3000);
            return new HealthMonitorResponse { Id = request.Id };
        }

        public bool Exception(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ProcessingException<ErrorLogEvent>(ErrorContexts.StringNullError);

            return true;
        }
    }
}