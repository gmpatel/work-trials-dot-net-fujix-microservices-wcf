using System;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Framework.Service.WCF.Proxy.Audit
{
    public interface IAuditProxy
    {
        BusinessResult AuditAsync(string trackingId, string externalCorrelationId, string documentReferenceNumber,
            string name, string description, string resource, string channelType, string machineName, string serviceName,
            string operationName, string operatorName);

        BusinessResult AuditAsync(string trackingId, string externalCorrelationId, string documentReferenceNumber,
            string name, string description, string resource, string channelType, string serviceName,
            string operationName, string operatorName);
        
        BusinessResult AuditAsync(AuditProxyRequest auditProxyRequest);
    }
}