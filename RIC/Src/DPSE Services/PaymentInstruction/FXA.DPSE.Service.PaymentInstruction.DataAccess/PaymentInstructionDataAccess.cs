using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.PaymentInstruction.Common;

namespace FXA.DPSE.Service.PaymentInstruction.DataAccess
{
    public class PaymentInstructionDataAccess : IPaymentInstructionDataAccess
    {
        private readonly PaymentInstructionDb _paymentInstructionDataContext;
        private readonly IAuditProxy _auditProxy;
        private readonly ILoggingProxy _loggingProxy;

        public PaymentInstructionDataAccess(DbContext paymentInstructionDataContext
            , IAuditProxy auditProxy, ILoggingProxy loggingProxy)
        {
            _paymentInstructionDataContext = (PaymentInstructionDb)paymentInstructionDataContext;
            _auditProxy = auditProxy;
            _loggingProxy = loggingProxy;
        }

        public void UpdatePaymentInstructionProcessingDate(long id, DateTime processingDate)
        {
                    var pi = _paymentInstructionDataContext.PaymentInstructions.FirstOrDefault(ip => ip.Id == id);
                    if (pi == null)
                    {
                            throw new ProcessingException<PaymentInstructionError>(PaymentInstructionError.EntityNotFoundError,
                                 string.Format("Payment instrucrtion Id {0} not found", id), string.Empty);
                    }

                    pi.ProcessingDateTime = processingDate;
                    pi.Status = "READY";
                    _paymentInstructionDataContext.PaymentInstructions.AddOrUpdate(pi);
                    _paymentInstructionDataContext.SaveChanges();

                    var postingCheques = pi.Vouchers.Where(e => !e.IsNonPostingCheque).ToList();

                    if (!postingCheques.Any())
                        throw new ProcessingException<PaymentInstructionError>(PaymentInstructionError.EntityNotFoundError,
                             string.Format("Voucher for payment instrucrtion Id: {0} not found", id), string.Empty);


                    foreach (var cheque in postingCheques)
                    {
                        cheque.ProcessingDateTime = processingDate;
                        _paymentInstructionDataContext.Vouchers.AddOrUpdate(cheque);
                    }
                    _paymentInstructionDataContext.SaveChanges();

                    var auditResource = new StringBuilder();
                    auditResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                    auditResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                    auditResource.AppendFormat("Tables:{0}, {1}{2}", _paymentInstructionDataContext.GetTableName<PaymentInstruction>(), _paymentInstructionDataContext.GetTableName<Voucher>(), Environment.NewLine);
                    var vouchersId = (_paymentInstructionDataContext.Vouchers.Any()) ? _paymentInstructionDataContext.Vouchers.Select(e => e.Id.ToString()).ToList().Aggregate((i, j) => i + ',' + j) : string.Empty;
                    auditResource.AppendFormat("Resources: PaymentInstruction{0} Vouchers:{1}", id, vouchersId);

                    
                    var auditResult = _auditProxy.AuditAsync(pi.TrackingId, "DatabaseAccess",
                        "Read access to payment instruction database", auditResource.ToString(), pi.ChannelType,
                        pi.ClientSession.SessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                    if (auditResult.HasException)
                        throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                            auditResult.BusinessException.ErrorCode);
                }
   

        public Account GetOrCreateAccount(Account account, IList<string> accountNames, string trackingId, string channelType, string sessionId)
        {
      
              
                    var acc = _paymentInstructionDataContext.Accounts.FirstOrDefault(a =>
                        a.AccountNumber.Equals(account.AccountNumber, StringComparison.CurrentCultureIgnoreCase) &&
                        a.BSBCode.Equals(account.BSBCode, StringComparison.CurrentCultureIgnoreCase) &&
                        a.AccountType.Equals(account.AccountType, StringComparison.CurrentCultureIgnoreCase));

                    var auditReadResource = new StringBuilder();
                    auditReadResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                    auditReadResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                    auditReadResource.AppendFormat("Tables:{0}, {1}{2}", _paymentInstructionDataContext.GetTableName<Account>(), _paymentInstructionDataContext.GetTableName<Voucher>(), Environment.NewLine);
                    auditReadResource.AppendFormat("Resources: AccountId {0}", account.Id);

                    var accountAccessAuditResult = _auditProxy.AuditAsync(trackingId, "DatabaseAccess",
                    "Read access to payment instruction database", auditReadResource.ToString(), channelType,
                    sessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                    if (accountAccessAuditResult.HasException)
                        throw new ProcessingException<ProxyError>(ProxyError.AuditError, accountAccessAuditResult.BusinessException.Message,
                            accountAccessAuditResult.BusinessException.ErrorCode);

                    if (acc == null)
                    {
                        _paymentInstructionDataContext.Accounts.Add(account);
                        _paymentInstructionDataContext.SaveChanges();

                        var auditResult = _auditProxy.AuditAsync(trackingId, "DatabaseAccess",
                   "Insert access to payment instruction database", auditReadResource.ToString(), channelType,
                   sessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                        if (auditResult.HasException)
                            throw new ProcessingException<ProxyError>(ProxyError.AuditError, auditResult.BusinessException.Message,
                                auditResult.BusinessException.ErrorCode);
                    }
                    else
                    {
                        account = acc;
                    }

                    foreach (var entry in accountNames)
                    {
                        var name = _paymentInstructionDataContext.AccountNames.FirstOrDefault(n =>
                            n.AccountId == account.Id &&
                            n.AccountName1.Equals(entry, StringComparison.CurrentCultureIgnoreCase)
                            );

                        if (name != null) continue;
                        var accountName = new AccountName
                        {
                            AccountId = account.Id,
                            AccountName1 = entry
                        };

                        _paymentInstructionDataContext.AccountNames.Add(accountName);
                        _paymentInstructionDataContext.SaveChanges();

                        var auditInsertResource = new StringBuilder();
                        auditInsertResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                        auditInsertResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                        auditInsertResource.AppendFormat("Tables:{0}, {1}", _paymentInstructionDataContext.GetTableName<AccountName>(), Environment.NewLine);
                        auditInsertResource.AppendFormat("Resources: AccountId:{0} AccountNames{1}",
                            account.Id, accountNames.Aggregate((i, j) => i + ',' + j));

                        var accountAccessauditResult = _auditProxy.AuditAsync(trackingId, "DatabaseAccess",
                        "Read access to payment instruction database", auditInsertResource.ToString(), channelType,
                        sessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                        if (accountAccessauditResult.HasException)
                            throw new ProcessingException<ProxyError>(ProxyError.AuditError, accountAccessauditResult.BusinessException.Message,
                                accountAccessauditResult.BusinessException.ErrorCode);
                    }

                
         
            return account;
        }

        public ClientSession CreateSession(ClientSession session, string trackingId, string channelType)
        {
                _paymentInstructionDataContext.ClientSessions.Add(session);
                var id = _paymentInstructionDataContext.SaveChanges();

                var auditReadResource = new StringBuilder();
                auditReadResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                auditReadResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                auditReadResource.AppendFormat("Tables:{0}{1}", _paymentInstructionDataContext.GetTableName<ClientSession>(), Environment.NewLine);
                auditReadResource.AppendFormat("Resources: SessionId {0}", id);

                var accountAccessAuditResult = _auditProxy.AuditAsync(trackingId, "DatabaseAccess",
                "Insert access to payment instruction database", auditReadResource.ToString(), channelType,
                session.SessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                if (accountAccessAuditResult.HasException)
                    throw new ProcessingException<ProxyError>(ProxyError.AuditError, accountAccessAuditResult.BusinessException.Message,
                        accountAccessAuditResult.BusinessException.ErrorCode);

                return session;

            
            
        }

        public PaymentInstruction CreatePaymentInstruction(PaymentInstruction paymentInstruction)
        {
                paymentInstruction = _paymentInstructionDataContext.PaymentInstructions.Add(paymentInstruction);
                var id =_paymentInstructionDataContext.SaveChanges();

                var auditReadResource = new StringBuilder();
                auditReadResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                auditReadResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                auditReadResource.AppendFormat("Tables:{0}{1}", _paymentInstructionDataContext.GetTableName<PaymentInstruction>(), Environment.NewLine);
                auditReadResource.AppendFormat("Resources: PaymentInstructionId {0}", id);

                var accountAccessAuditResult = _auditProxy.AuditAsync(paymentInstruction.TrackingId, "DatabaseAccess",
                "Insert access to payment instruction database", auditReadResource.ToString(), paymentInstruction.ChannelType,
                paymentInstruction.ClientSession.SessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                if (accountAccessAuditResult.HasException)
                    throw new ProcessingException<ProxyError>(ProxyError.AuditError, accountAccessAuditResult.BusinessException.Message,
                        accountAccessAuditResult.BusinessException.ErrorCode);

                return paymentInstruction;
            
            
        }

        public IDictionary<Voucher, VoucherImage> SaveVouchers(IDictionary<Voucher, VoucherImage> vouchers, string trackingId, string channelType, string sessionId)
        {
                var result = new Dictionary<Voucher, VoucherImage>();

                foreach (var entry in vouchers)
                {
                    var voucher = entry.Key;
                    var voucherImage = entry.Value;

                    _paymentInstructionDataContext.Vouchers.Add(voucher);
                    _paymentInstructionDataContext.SaveChanges();

                    voucherImage.VoucherId = voucher.Id;
                    _paymentInstructionDataContext.VoucherImages.Add(voucherImage);
                    _paymentInstructionDataContext.SaveChanges();

                    result.Add(voucher, voucherImage);
                }

                var auditReadResource = new StringBuilder();
                auditReadResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                auditReadResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                auditReadResource.AppendFormat("Tables: Vouchers{0}, VoucherImages:{1}{2}", _paymentInstructionDataContext.GetTableName<Voucher>(), _paymentInstructionDataContext.GetTableName<VoucherImage>(), Environment.NewLine);
                auditReadResource.AppendFormat("Resources: Voucher Ids {0}{1}", result.Keys.Select(e=>e.Id).Aggregate((i, j) => i + ',' + j), Environment.NewLine);

                var accountAccessAuditResult = _auditProxy.AuditAsync(trackingId, "DatabaseAccess",
                "Insert access to payment instruction database", auditReadResource.ToString(), channelType,
                sessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                if (accountAccessAuditResult.HasException)
                    throw new ProcessingException<ProxyError>(ProxyError.AuditError, accountAccessAuditResult.BusinessException.Message,
                        accountAccessAuditResult.BusinessException.ErrorCode);

                return result;      
            
          
        }

        public PaymentInstruction UpdatePaymentInstructionStatus(PaymentInstruction paymentInstruction)
        {
                var pi = _paymentInstructionDataContext.PaymentInstructions.FirstOrDefault(p =>
                    p.ChannelType.Equals(paymentInstruction.ChannelType, StringComparison.CurrentCultureIgnoreCase) &&
                    p.TrackingId.Equals(paymentInstruction.TrackingId, StringComparison.CurrentCultureIgnoreCase)
                );

                if (pi == null)
                {
                    throw new ProcessingException<PaymentInstructionError>(PaymentInstructionError.EntityNotFoundError,
                                string.Format("Payment instrucrtion Id {0} not found", paymentInstruction.Id), string.Empty);
                }

                pi.Status = paymentInstruction.Status;
                _paymentInstructionDataContext.PaymentInstructions.AddOrUpdate(pi);
                _paymentInstructionDataContext.SaveChanges();

                var auditReadResource = new StringBuilder();
                auditReadResource.AppendFormat("Server:{0}{1}", _paymentInstructionDataContext.Database.Connection.DataSource, Environment.NewLine);
                auditReadResource.AppendFormat("Database:{0}{1}", _paymentInstructionDataContext.Database.Connection.Database, Environment.NewLine);
                auditReadResource.AppendFormat("Tables:{0}{1}", _paymentInstructionDataContext.GetTableName<PaymentInstruction>(), Environment.NewLine);
                auditReadResource.AppendFormat("Resources: PaymentInstructionId {0}", paymentInstruction.Id);

                var accountAccessAuditResult = _auditProxy.AuditAsync(paymentInstruction.TrackingId, "DatabaseAccess",
                "Update access to payment instruction database", auditReadResource.ToString(), paymentInstruction.ChannelType,
                paymentInstruction.ClientSession.SessionId, Environment.MachineName, "PaymentInstruction", "PaymentInstruction");

                if (accountAccessAuditResult.HasException)
                    throw new ProcessingException<ProxyError>(ProxyError.AuditError, accountAccessAuditResult.BusinessException.Message,
                        accountAccessAuditResult.BusinessException.ErrorCode);

                return pi;    
            
        }

        public void Dispose()
        {
            if (_paymentInstructionDataContext != null)
                _paymentInstructionDataContext.Dispose();
        }
    }
}