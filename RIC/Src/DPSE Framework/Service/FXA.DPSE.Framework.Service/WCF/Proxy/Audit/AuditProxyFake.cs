using System;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Framework.Service.WCF.Proxy.Audit
{
    public class AuditProxyFake : IAuditProxy
    {
        public BusinessResult AuditAsync(string trackingId, string externalCorrelationId, string documentReferenceNumber,
            string name, string description, string resource, string channelType, string machineName, string serviceName,
            string operationName, string operatorName)
        {
            return new BusinessResult();
        }

        public BusinessResult AuditAsync(string trackingId, string externalCorrelationId, string documentReferenceNumber,
            string name, string description, string resource, string channelType, string serviceName,
            string operationName, string operatorName)
        {
            return new BusinessResult();
        }

        public BusinessResult AuditAsync(AuditProxyRequest auditProxyRequest)
        {
            return new BusinessResult();
        }
    }
}