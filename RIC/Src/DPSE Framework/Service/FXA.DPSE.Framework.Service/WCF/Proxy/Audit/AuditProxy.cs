using System;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DTO.Audit;

namespace FXA.DPSE.Framework.Service.WCF.Proxy.Audit
{
    public class AuditProxy : IAuditProxy 
    {
        private readonly IFrameworkConfig _frameworkConfig;

        public AuditProxy(IFrameworkConfig frameworkConfig)
        {
            _frameworkConfig = frameworkConfig;
        }

        public BusinessResult AuditAsync(string trackingId, string externalCorrelationId, string documentReferenceNumber, string name, string description, string resource, string channelType, string machineName, string serviceName, string operationName, string operatorName)
        {
            var result = new BusinessResult();
            if (!_frameworkConfig.Services.AuditService.Enabled) return result;

            var auditRequest = new AuditRequest()
            {
                TrackingId = trackingId,
                ExternalCorrelationId = externalCorrelationId,
                DocumentReferenceNumber = documentReferenceNumber,
                AuditDateTime = string.Format("{0}Z", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff").Replace(" ", "T")),
                Name = name,
                Description = description,
                Resource = resource,
                ChannelType = channelType,
                MachineName = machineName,
                ServiceName = serviceName,
                OperationName = operationName,
                OperatorName = operatorName
            };

            var httpAuditResponse =
                HttpClientExtensions.PostSyncAsJson<AuditRequest, AuditResponse>(
                    _frameworkConfig.Services.AuditService.Url, auditRequest);
            var auditResponse = httpAuditResponse.Content;

            if (!httpAuditResponse.Succeeded)
            {
                result.AddBusinessException(
                    new DpseBusinessException(
                        DpseBusinessExceptionType.AuditServiceException,
                        (auditResponse != null) ? auditResponse.Code : "DPSE-1003",
                        (auditResponse != null) ? auditResponse.Message : "A critical error occurred processing the request", 
                        string.Empty));
            }

            return result;
        }


        public BusinessResult AuditAsync(string trackingId, string externalCorrelationId, string documentReferenceNumber, string name, string description, string resource, string channelType, string serviceName, string operationName, string operatorName)
        {
            return AuditAsync(trackingId, externalCorrelationId, documentReferenceNumber, name, description, resource, channelType, Environment.MachineName, serviceName, operationName, operatorName);
        }

        public BusinessResult AuditAsync(AuditProxyRequest auditProxyRequest)
        {
            if (string.IsNullOrEmpty(auditProxyRequest.MachineName))
            {
                return AuditAsync(auditProxyRequest.TrackingId, auditProxyRequest.ExternalCorrelationId, auditProxyRequest.DocumentReferenceNumber,
                auditProxyRequest.Name, auditProxyRequest.Description, auditProxyRequest.Resource,
                auditProxyRequest.ChannelType, auditProxyRequest.ServiceName,
                auditProxyRequest.OperationName, auditProxyRequest.OperatorName);
            }
            
            return AuditAsync(auditProxyRequest.TrackingId, auditProxyRequest.ExternalCorrelationId, auditProxyRequest.DocumentReferenceNumber, 
                auditProxyRequest.Name, auditProxyRequest.Description, auditProxyRequest.Resource, 
                auditProxyRequest.ChannelType, auditProxyRequest.MachineName, auditProxyRequest.ServiceName, 
                auditProxyRequest.OperationName, auditProxyRequest.OperatorName);
        }
    }
}