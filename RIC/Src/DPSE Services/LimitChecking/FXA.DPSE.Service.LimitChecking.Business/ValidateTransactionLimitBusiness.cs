using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.LimitChecking.Business.BusinessEntity;
using FXA.DPSE.Service.LimitChecking.Common;
using FXA.DPSE.Service.LimitChecking.Common.Configuration;

namespace FXA.DPSE.Service.LimitChecking.Business
{
    public class ValidateTransactionLimitBusiness : IValidateTransactionLimitBusiness
    {
        private readonly ILimitCheckingServiceConfiguration _limitCheckingServiceConfiguration;
        private readonly IAuditProxy _auditProxy;

        public ValidateTransactionLimitBusiness(
            ILimitCheckingServiceConfiguration serviceConfiguration,
            IAuditProxy auditProxy)
        {
            _limitCheckingServiceConfiguration = serviceConfiguration;
            _auditProxy = auditProxy;
        }

        public TransactionLimitValidationResult ValidatePayloadTransactionLimit(ChequePayload chequePayload)
        {
            var result = new TransactionLimitValidationResult {Cheques = new List<DepositChequeResult>()};

            ValidateChequeLimit(chequePayload, result);

            var totalTransactionAmount = chequePayload.Cheques.Sum(cheque => cheque.ChequeAmount);
            var transactionLimitExceeded = totalTransactionAmount > _limitCheckingServiceConfiguration.TransactionLimit.Amount;

            if (transactionLimitExceeded)
            {
                var businessErrorDescription = string.Format("Total transaction amount: {0} is greater than {1}",
                    totalTransactionAmount, _limitCheckingServiceConfiguration.TransactionLimit.Amount);

                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.BusinessRule,
                    StatusCode.LimitCheckExceeded, businessErrorDescription, "Transaction limit exceeded"));

                AuditLimitChecking(chequePayload, "TransactionLimitExceed", businessErrorDescription, result);
                
                return result;
            }

            AuditLimitChecking(chequePayload, "TransactionLimitValidationPassed",
                string.Format("Total transaction amount: {0} is not greater than {1}", totalTransactionAmount,
                    _limitCheckingServiceConfiguration.TransactionLimit.Amount), result);

            return result;
        }

        private void AuditLimitChecking(ChequePayload chequePayload, string aduitMessage, string businessErrorDescription, TransactionLimitValidationResult result)
        {
            var auditResult = PublishAuditEvent(chequePayload, aduitMessage, businessErrorDescription);

            if (auditResult.HasException)
            {
                if (result.Cheques != null) result.Cheques.Clear();

                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.AuditServiceException,
                    StatusCode.InternalProcessingError, auditResult.BusinessException.Message, auditResult.BusinessException.Details));
            }
        }

        private void ValidateChequeLimit(ChequePayload chequePayload, TransactionLimitValidationResult result)
        {
            foreach (var cheque in chequePayload.Cheques)
            {
                if (cheque.ChequeAmount > _limitCheckingServiceConfiguration.TransactionLimit.Amount)
                {
                    result.Cheques.Add(
                        new DepositChequeResult(
                            StatusCode.LimitCheckExceeded, "Transaction limit exceeded", cheque.TrackingId,
                            cheque.SequenceId, cheque.ChequeAmount, cheque.Codeline));
                }
                else
                {
                    result.Cheques.Add(
                        new DepositChequeResult(
                            StatusCode.LimitCheckSuccessful,
                            "Limit check successful", cheque.TrackingId, cheque.SequenceId, cheque.ChequeAmount,
                            cheque.Codeline));
                }
            }
        }

        private BusinessResult PublishAuditEvent(ChequePayload chequePayload, string name, string description)
        {
            var auditResult = new BusinessResult();
            if (!string.IsNullOrEmpty(chequePayload.TrackingId))
            {
                auditResult = _auditProxy.AuditAsync(
                    new AuditProxyRequest
                    {
                        ChannelType = chequePayload.ChannelType,
                        Description = description,
                        MachineName = Environment.MachineName,
                        Name = name,
                        OperationName = "CheckTransactionLimit",
                        Resource = string.Empty,
                        ServiceName = "LimitChecking",
                        SessionId = chequePayload.SessionId,
                        TrackingId = chequePayload.TrackingId
                    });
            }
            return auditResult;
        }
    }
}