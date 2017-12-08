using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Service.DipsTransport.Common.TransportException;

namespace FXA.DPSE.Service.DipsTransport.DataAccess
{
    public class DipsTransportDataAccess : IDipsTransportDataAccess
    {
        private readonly DbContext _paymentInstructionContext;
        private readonly PaymentInstructionDb _paymentInstructionData;
        private readonly DipsTransportDb _dipsTransportData;
        private readonly DbContext _dipsTransportContext;
        private readonly IAuditProxy _auditProxy;

        public DipsTransportDataAccess(DbContext paymentInstructionData, DbContext dipsTransportData, IAuditProxy auditProxy)
        {
            _paymentInstructionContext = paymentInstructionData;
            _paymentInstructionData = (PaymentInstructionDb) paymentInstructionData;
            _dipsTransportContext = dipsTransportData;
            _dipsTransportData = (DipsTransportDb) dipsTransportData;
            _auditProxy = auditProxy;
        }

        public void Dispose()
        {
            if (_paymentInstructionData != null) _paymentInstructionData.Dispose();
            if(_dipsTransportData != null) _dipsTransportData.Dispose();
        }

        public IList<Transmission> GetTransportedTransmissions(DateTime? dateTime)
        {
            try
            {
                List<Transmission> result;

                if (dateTime != null)
                {
                    result = _dipsTransportData.Transmissions.Where(x =>
                        x.Status.Equals("COMPLETED", StringComparison.InvariantCultureIgnoreCase) &&
                        x.TransportedDateTime.HasValue &&
                        SqlFunctions.DateDiff("dd", dateTime, x.TransportedDateTime) == 0
                        ).ToList();
                }
                else
                {
                    result = _dipsTransportData.Transmissions.Where(x =>
                        x.Status.Equals("COMPLETED", StringComparison.InvariantCultureIgnoreCase) &&
                        x.TransportedDateTime.HasValue
                    ).ToList();

                }

                return result;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                       exception.Message, exception, string.Empty);
            }
        } 

        public bool UpdatePaymentInstructionsTransmitted(Transmission transmission, DateTime? dateTime)
        {
            try
            {
                foreach (var batch in transmission.TransmissionBatches)
                {
                    var paymentInstruction = GetPaymentInstruction(batch.PaymentInstructionId);
                    paymentInstruction.TransportedDateTime = dateTime ?? DateTime.UtcNow;
                    paymentInstruction.Status = "COMPLETED";
                    _paymentInstructionData.PaymentInstructions.AddOrUpdate(paymentInstruction);
                    _paymentInstructionData.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }
        }

        public bool MarkBatchedPaymentInstructionsAsPicked(IList<PaymentInstruction> paymentInstructions)
        {
            try
            {
                paymentInstructions.ToList().ForEach(x => x.Status = "PICKED");
                paymentInstructions.ToList().ForEach(x => _paymentInstructionData.PaymentInstructions.AddOrUpdate(x));
                _paymentInstructionData.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool UpdateTransmission(long id, FileInfo file, string fileSHA)
        {
            try
            {
                var transmission = _dipsTransportData.Transmissions.FirstOrDefault(x => x.Id == id);

                if (transmission != null)
                {
                    transmission.FileName = file.Name;
                    transmission.FilePath = Path.GetDirectoryName(file.FullName);
                    transmission.FileSHAHash = fileSHA;

                    _dipsTransportData.Transmissions.AddOrUpdate(transmission);
                    _dipsTransportData.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }
        }

        public bool UpdateTransmission(Transmission transmission)
        {
            try
            {
                _dipsTransportData.Transmissions.AddOrUpdate(transmission);
                _dipsTransportData.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }
        }

        public bool UpdateTransmissionBatch(TransmissionBatch transmissionBatch)
        {
            try
            {
                _dipsTransportData.TransmissionBatches.AddOrUpdate(transmissionBatch);
                _dipsTransportData.SaveChanges();
            
                return true;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }
        }
        public bool UpdateTransmissionBatchWithFileInfo(long transmissionId, long paymentInstructionId, FileInfo file)
        {
            try
            {
                var transmissionBatch = _dipsTransportData.TransmissionBatches.FirstOrDefault(x => x.TransmissionId == transmissionId && x.PaymentInstructionId == paymentInstructionId);

                if (transmissionBatch != null)
                {
                    transmissionBatch.FileName = file.Name;
                    transmissionBatch.FilePath = Path.GetDirectoryName(file.FullName);
                
                    _dipsTransportData.TransmissionBatches.AddOrUpdate(transmissionBatch);
                    _dipsTransportData.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }
        }

        public PaymentInstruction GetPaymentInstruction(long id)
        {
            var paymentInstruction = default(PaymentInstruction);

            try
            {
                paymentInstruction = _paymentInstructionData.PaymentInstructions.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }

            /*
            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", _paymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", _paymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionContext.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            var resources = (paymentInstructions.Count > 0) ? paymentInstructions.Select(e => e.Id.ToString()).ToList().Aggregate((i, j) => i + ',' + j) : string.Empty;
            auditResource.AppendFormat("Resources:{0}", resources);


            if (paymentInstructions == null || !paymentInstructions.Any())
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.EntityNotFoundError,
                     string.Format("Payment instruction not found"), string.Empty);

            var auditResult = _auditProxy.AuditAsync(paymentInstructions.First().TrackingId, "DatabaseAccess",
                "Read access to payment instruction database", auditResource.ToString(), paymentInstructions.First().ChannelType,
                paymentInstructions.First().ClientSession.SessionId, Environment.MachineName, "DipsTransport", "Payload");

            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);
            */

            return paymentInstruction;
        }

        public IList<Transmission> GetTransportTransmissionsToBeProcessed()
        {
            var transmissions = default(IList<Transmission>);

            try
            {
                transmissions = _dipsTransportData.Transmissions.Where(x => 
                    x.TransportedDateTime.HasValue == false &&
                    x.Status.Equals("COMPLETED", StringComparison.CurrentCultureIgnoreCase) == false
                ).ToList();
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }

            /*
            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", _paymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", _paymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionContext.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            var resources = (paymentInstructions.Count > 0) ? paymentInstructions.Select(e => e.Id.ToString()).ToList().Aggregate((i, j) => i + ',' + j) : string.Empty;
            auditResource.AppendFormat("Resources:{0}", resources);


            if (paymentInstructions == null || !paymentInstructions.Any())
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.EntityNotFoundError,
                     string.Format("Payment instruction not found"), string.Empty);

            var auditResult = _auditProxy.AuditAsync(paymentInstructions.First().TrackingId, "DatabaseAccess",
                "Read access to payment instruction database", auditResource.ToString(), paymentInstructions.First().ChannelType,
                paymentInstructions.First().ClientSession.SessionId, Environment.MachineName, "DipsTransport", "Payload");

            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);
            */

            return transmissions;
        }

        public Transmission CreateTransportTransmissionRecords(IList<PaymentInstruction> paymentInstructions)
        {
            var transmission = default(Transmission);

            try
            {
                if (paymentInstructions != null && paymentInstructions.Count > 0)
                {
                    transmission = new Transmission
                    {
                        CreationDateTime = DateTime.UtcNow,
                        TransactionCount = 0,
                        RetryCount = 0,
                        Status = "INIT"
                    };

                    _dipsTransportData.Transmissions.Add(transmission);
                    _dipsTransportData.SaveChanges();

                    var totalTransactions = 0;

                    foreach (var paymentInstruction in paymentInstructions)
                    {
                        var transmissionBatch = new TransmissionBatch
                        {
                            TransmissionId = transmission.Id,
                            PaymentInstructionId = paymentInstruction.Id,
                            BatchNumber = paymentInstruction.BatchNumber,
                            TransactionCount = paymentInstruction.ChequeCount
                        };

                        _dipsTransportData.TransmissionBatches.Add(transmissionBatch);
                        _dipsTransportData.SaveChanges();

                        totalTransactions = totalTransactions + paymentInstruction.ChequeCount;
                    }

                    transmission.TransactionCount = totalTransactions;
                    _dipsTransportData.Transmissions.AddOrUpdate(transmission);
                    _dipsTransportData.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }

            /*
            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", _paymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", _paymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionContext.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            var resources = (paymentInstructions.Count > 0) ? paymentInstructions.Select(e => e.Id.ToString()).ToList().Aggregate((i, j) => i + ',' + j) : string.Empty;
            auditResource.AppendFormat("Resources:{0}", resources);


            if (paymentInstructions == null || !paymentInstructions.Any())
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.EntityNotFoundError,
                     string.Format("Payment instruction not found"), string.Empty);

            var auditResult = _auditProxy.AuditAsync(paymentInstructions.First().TrackingId, "DatabaseAccess",
                "Read access to payment instruction database", auditResource.ToString(), paymentInstructions.First().ChannelType,
                paymentInstructions.First().ClientSession.SessionId, Environment.MachineName, "DipsTransport", "Payload");

            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);
            */

            return transmission;
        }

        public IList<PaymentInstruction> GetBatchedPaymentInstructions()
        {
            List<PaymentInstruction> paymentInstructions;
            try
            {
                paymentInstructions = _paymentInstructionData.PaymentInstructions.Where(x =>
                    x.Status.Equals("BATCHED", StringComparison.CurrentCultureIgnoreCase) &&
                    x.TransportedDateTime.HasValue == false &&
                    x.ProcessingDateTime.HasValue &&
                    string.IsNullOrEmpty(x.BatchPath) == false &&
                    SqlFunctions.DateDiff("dd", DateTime.Now, x.ProcessingDateTime) <= 0
                ).ToList();

                paymentInstructions.ForEach(x => x.Status = "PICKED");
                paymentInstructions.ForEach(x => _paymentInstructionData.PaymentInstructions.AddOrUpdate(x));
                _paymentInstructionData.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.DatabaseAccessError,
                      exception.Message, exception, string.Empty);
            }

            /*
            var auditResource = new StringBuilder();
            auditResource.AppendFormat("Server:{0}{1}", _paymentInstructionData.Database.Connection.DataSource, Environment.NewLine);
            auditResource.AppendFormat("Database:{0}{1}", _paymentInstructionData.Database.Connection.Database, Environment.NewLine);
            auditResource.AppendFormat("Tables:{0}{1}", _paymentInstructionContext.GetTableNames().Aggregate((i, j) => i + ',' + j), Environment.NewLine);
            var resources = (paymentInstructions.Count > 0) ? paymentInstructions.Select(e => e.Id.ToString()).ToList().Aggregate((i, j) => i + ',' + j) : string.Empty;
            auditResource.AppendFormat("Resources:{0}", resources);


            if (paymentInstructions == null || !paymentInstructions.Any())
                throw new ProcessingException<DipsTransportErrorType>(DipsTransportErrorType.EntityNotFoundError,
                     string.Format("Payment instruction not found"), string.Empty);

            var auditResult = _auditProxy.AuditAsync(paymentInstructions.First().TrackingId, "DatabaseAccess",
                "Read access to payment instruction database", auditResource.ToString(), paymentInstructions.First().ChannelType,
                paymentInstructions.First().ClientSession.SessionId, Environment.MachineName, "DipsTransport", "Payload");

            if (auditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                    auditResult.BusinessException.ErrorCode);
            */

            return paymentInstructions;
        }

        public IList<PaymentInstruction> GetPayloads(Guid? requestId)
        {
            try
            {
                return (from 
                        p in _paymentInstructionData.PaymentInstructions
                    where 
                        p.Status.Equals("BATCHED", StringComparison.CurrentCultureIgnoreCase) && 
                        p.ProcessingDateTime.HasValue &&
                        p.BatchCreatedDateTime.HasValue &&
                        p.TransportedDateTime.HasValue == false &&
                        SqlFunctions.DateDiff("dd", p.ProcessingDateTime.Value, DateTime.UtcNow) == 0 &&
                        string.IsNullOrEmpty(p.BatchPath) == false
                    orderby 
                        p.ProcessingDateTime
                    select 
                        p
                ).ToList();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public bool UpdatePayloadsTransported(long id)
        {
            var p = _paymentInstructionData.PaymentInstructions.FirstOrDefault(x => x.Id == id);

            if (p != null)
            {
                p.TransportedDateTime = DateTime.UtcNow;
                _paymentInstructionData.PaymentInstructions.AddOrUpdate(p);
                _paymentInstructionData.SaveChanges();
            }

            return true;
        }
    }
    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        public static IEnumerable<string> GetTableNames(this DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            var tables = metadata.GetItemCollection(DataSpace.SSpace)
                .GetItems<EntityContainer>()
                .Single()
                .BaseEntitySets
                .OfType<EntitySet>()
                .Where(s => !s.MetadataProperties.Contains("Type")
                            || s.MetadataProperties["Type"].ToString() == "Tables");

            var list = new List<string>();
            foreach (var table in tables)
            {
                var tableName = table.MetadataProperties.Contains("Table")
                                && table.MetadataProperties["Table"].Value != null
                    ? table.MetadataProperties["Table"].Value.ToString()
                    : table.Name;
                list.Add(tableName);
            }
            return list;
        }
    }
}