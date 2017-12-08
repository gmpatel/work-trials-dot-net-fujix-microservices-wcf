using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Service.DipsPayload.Common.PayloadException;

namespace FXA.DPSE.Service.DipsPayload.DataAccess
{
    public class PaymentInstructionDataAccess : IPaymentInstructionDataAccess
    {
        private readonly DbContext _paymentInstructionDataContxt;
        private readonly IAuditProxy _auditProxy;
        private PaymentInstructionDb PaymentInstructionData { get; set; }

        public PaymentInstructionDataAccess(DbContext paymentInstructionDataContext, IAuditProxy auditProxy)
        {
            _paymentInstructionDataContxt = paymentInstructionDataContext;
            _auditProxy = auditProxy;
            PaymentInstructionData = (PaymentInstructionDb)paymentInstructionDataContext;
        }

        /*TODO: 
         * This should NOT be disposing here in the business logic at all. We are not controlling the lifecyle of the objects when using IoC.
           We just need to inform IoC how it needs to dispose objects.
         * Update tracking id for audit service in batch all and batch single.
         */

        public void Dispose()
        {
            if (PaymentInstructionData != null) PaymentInstructionData.Dispose();
        }

        public IList<PaymentInstruction> GetPaymentInstructions()
        {
            List<PaymentInstruction> paymentInstructions;
            try
            {
                paymentInstructions = PaymentInstructionData.PaymentInstructions.Where(x =>
                    x.Status.Equals("READY", StringComparison.CurrentCultureIgnoreCase) &&
                    x.ProcessingDateTime.HasValue &&
                    //SqlFunctions.DateDiff("dd", DateTime.Now, x.ProcessingDateTime) <= 0 &&
                    x.BatchCreatedDateTime.HasValue == false &&
                    string.IsNullOrEmpty(x.BatchPath)
                ).ToList();
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }
            
            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", PaymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", PaymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionDataContxt.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            var resources = (paymentInstructions.Count > 0) ? paymentInstructions.Select(e => e.Id.ToString()).ToList().Aggregate((i, j) => i + ',' + j) : string.Empty;
            auditResource.AppendFormat("Resources:{0}", resources);

            //if (paymentInstructions == null || ! paymentInstructions.Any())
            //    throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.EntityNotFoundError,
            //         string.Format("No Payment Instructions found to be processed today"), string.Empty);

            if (paymentInstructions.Any())
            {
                var auditResult = _auditProxy.AuditAsync(paymentInstructions.First().TrackingId, "DatabaseAccess",
                    "Read access to payment instruction database", auditResource.ToString(), paymentInstructions.First().ChannelType,
                    paymentInstructions.First().ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

                if (auditResult.HasException)
                    throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                        auditResult.BusinessException.ErrorCode);    
            }
            
            return paymentInstructions;
        }

        public PaymentInstruction GetPaymentInstruction(long id)
        {
            PaymentInstruction paymentInstruction;
            try
            {
                paymentInstruction = PaymentInstructionData.PaymentInstructions.FirstOrDefault(x =>
                   x.Id == id &&
                   x.Status.Equals("READY", StringComparison.CurrentCultureIgnoreCase) &&
                   x.ProcessingDateTime.HasValue &&
                   x.BatchCreatedDateTime.HasValue == false &&
                   string.IsNullOrEmpty(x.BatchPath)
               );
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }

            if(paymentInstruction == null)
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.EntityNotFoundError,
                     string.Format("Payment instruction with ID:{0} not found", id), string.Empty);
            
            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", PaymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", PaymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionDataContxt.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            auditResource.AppendFormat("Resources:{0}", paymentInstruction.Id);

            var auditResult = _auditProxy.AuditAsync(paymentInstruction.TrackingId, "DatabaseAccess",
                "Read access to payment instruction database", auditResource.ToString(), paymentInstruction.ChannelType,
                paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");
            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);

            return paymentInstruction;

        }

        public bool UpdatePaymentInstructionBatchDetails(long id, DirectoryInfo directory, DateTime bathCreateDateTime)
        {
            var paymentInstruction = PaymentInstructionData.PaymentInstructions.FirstOrDefault(p => p.Id == id);

            if (paymentInstruction == null) return false;
            paymentInstruction.Status = "BATCHED";
            paymentInstruction.BatchPath = directory.FullName;
            paymentInstruction.BatchCreatedDateTime = bathCreateDateTime;
            paymentInstruction.BatchNumber = (paymentInstruction.Id % 100000000).ToString("00000000");
            PaymentInstructionData.SaveChanges();

            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", PaymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", PaymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionDataContxt.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            auditResource.AppendFormat("Resources:{0}", id);

            var auditResult = _auditProxy.AuditAsync(paymentInstruction.TrackingId, "DatabaseAccess",
                "Updated payment instruction database", auditResource.ToString(), paymentInstruction.ChannelType,
                paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);

            return true;
        }
    }
}