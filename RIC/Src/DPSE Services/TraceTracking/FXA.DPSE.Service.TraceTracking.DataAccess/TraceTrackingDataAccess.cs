using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Service.TraceTracking.DataAccess.Core;

namespace FXA.DPSE.Service.TraceTracking.DataAccess
{
    // TODO: 
    // Audit should be part of the EF to get detailed information and also get more reusability.
    // Need to get unit of work, DataContext (Change tracking) , and Repository in Framework.DataAccess.Core project which needs EF tempalte customization.
    // Need to implement some code in order to support read audit in the framework.
    // Need to support SaveChangesAsync in the project for couple of reason.
    // Need to register related things in Autofac in the different way in order to manage lifetime of the object efficiently.
    // Read could not be part of Audit or Resource could be ignored form Audit information.
    // Need to bind some common information around the current instance of the service to the OperationContext (i.e. implement IExtensions<T>) which need to be available 
    // in the different layers via Helper method.

    // Better to move Audit and Logging Proxy classes to DPSE.Framework.Common

    /* No Audit as there is no TrackingId in the incoming message.*/

    public class TraceTrackingDataAccess : ITraceTrackingDataAccess
    {
        private readonly IAuditProxy _auditProxy;
        private TraceTrackingDb TraceTrackingData { get; set; }

        public TraceTrackingDataAccess(DbContext traceTrackingDataContext, IAuditProxy auditProxy)
        {
            _auditProxy = auditProxy;
            TraceTrackingData = (TraceTrackingDb) traceTrackingDataContext;
        }

        public bool CheckRequestHasBeenProcessedRecently(int timeOutMiliseconds, ElectronicTraceTracking data, string sessionId)
        {
            var record = TraceTrackingData.ElectronicTraceTrackings.FirstOrDefault(tt =>
                tt.ClientIpAddressV4.Equals(data.ClientIpAddressV4, StringComparison.CurrentCultureIgnoreCase) &&
                tt.ClientDeviceId.Equals(data.ClientDeviceId, StringComparison.CurrentCultureIgnoreCase) &&
                tt.ChannelType.Equals(data.ChannelType, StringComparison.CurrentCultureIgnoreCase) &&
                tt.TotalTransactionAmount == data.TotalTransactionAmount &&
                tt.DepositAccountBsbCode == data.DepositAccountBsbCode &&
                tt.DepositAccountName == data.DepositAccountName &&
                tt.DepositAccountNumber == data.DepositAccountNumber &&
                tt.DepositAccountType == data.DepositAccountType &&
                SqlFunctions.DateDiff("dd", tt.DateTimeCreated, DateTime.UtcNow) <= 1 &&
                SqlFunctions.DateDiff("ms", tt.DateTimeCreated, DateTime.UtcNow) <= timeOutMiliseconds
            );

            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", TraceTrackingData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", TraceTrackingData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}", TraceTrackingData.GetTableName<ElectronicTraceTracking>());
            //auditResource.AppendFormat("Resources:{0}", resources);

            //var auditResult = _auditProxy.AuditAsync(string.Empty, "DatabaseAccess",
            //    "Read access to trace tracking database", auditResource.ToString(), data.ChannelType,
            //    sessionId, Environment.MachineName, "TraceTrackingService", "ElectronicTraceTracking");

            //if (auditResult.HasException)
            //    throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
            //        auditResult.BusinessException.ErrorCode);

            return record != null;
        }

        public IList<string> GenerateTraceTrackingNumber(ElectronicTraceTracking data, string sessionId)
        {
            var result = new List<string>();

            TraceTrackingData.ElectronicTraceTrackings.Add(data);
            TraceTrackingData.SaveChanges();

            var electronicTraceTrackingId = data.Id;
            //var trackingDetailIds = new List<int>();

            if (data.ChequeCount > 0)
            {
                for (var i = 0; i < data.ChequeCount; i++)
                {
                    var detailData = new ElectronicTraceTrackingDetail
                    {
                        ElectronicTraceTrackingId = electronicTraceTrackingId,
                    };

                    TraceTrackingData.ElectronicTraceTrackingDetails.Add(detailData);
                    var id = TraceTrackingData.SaveChanges();
                    //trackingDetailIds.Add(id);

                    result.Add(detailData.ChequeTraceTrackingCode);
                }
            }

            //var auditResource = new StringBuilder();
            //auditResource.AppendFormat("Server:{0}{1}", TraceTrackingData.Database.Connection.DataSource, Environment.NewLine);
            //auditResource.AppendFormat("Database:{0}{1}", TraceTrackingData.Database.Connection.Database, Environment.NewLine);
            //auditResource.AppendFormat("Tables:{0}{1}", TraceTrackingData.GetTableNames(), Environment.NewLine);

            //auditResource.AppendFormat("Resources: {0}ElectronicTraceTracking Table Id:{1}{2}", Environment.NewLine, electronicTraceTrackingId, Environment.NewLine);
            //auditResource.AppendFormat("ElectronicTraceTrackingDetail Table Ids:{0}{1}", trackingDetailIds.Aggregate((i, j) => i + ',' + j), Environment.NewLine);

            //var auditResult = _auditProxy.AuditAsync(string.Empty, "DatabaseAccess",
            //    "Insert access to trace tracking database", auditResource.ToString(), data.ChannelType,
            //    sessionId, Environment.MachineName, "TraceTrackingService", "ElectronicTraceTracking");

            //if (auditResult.HasException)
            //    throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message, auditResult.BusinessException.ErrorCode);

            return result;
        }
    }
}