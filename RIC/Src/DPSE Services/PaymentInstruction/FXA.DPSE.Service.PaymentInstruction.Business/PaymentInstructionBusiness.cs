using System;
using System.Collections.Generic;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.DataAccess;

namespace FXA.DPSE.Service.PaymentInstruction.Business
{
    public class PaymentInstructionBusiness : IPaymentInstructionBusiness
    {
        private readonly IPaymentInstructionServiceConfiguration _configuration;
        private readonly IPaymentInstructionDataAccess _paymentInstructionDataAccess;

        public PaymentInstructionBusiness(IPaymentInstructionServiceConfiguration configuration,
            IPaymentInstructionDataAccess paymentInstructionDataAccess)
        {
            _configuration = configuration;
            _paymentInstructionDataAccess = paymentInstructionDataAccess;
        }

        public PaymentInstructionStatusUpdateBusinessResult UpdateStatus(PaymentInstructionStatusUpdateBusinessData data)
        {
            var result = new PaymentInstructionStatusUpdateBusinessResult();
            try
            {
                
                var paymentInstruction = data.GetEntityPaymentInstruction();
                _paymentInstructionDataAccess.UpdatePaymentInstructionStatus(paymentInstruction);
                return result;
            }
            catch (ProcessingException<PaymentInstructionError> processingException)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, processingException.Message, string.Empty));

                if (processingException.Error == PaymentInstructionError.EntityNotFoundError)
                    result.BusinessException.ErrorCode = StatusCode.EntityNotFound;

                if (processingException.Error == PaymentInstructionError.DatabaseAccessError)
                    result.BusinessException.ErrorCode = StatusCode.DatabaseOrFileAccessError;
            }
            catch (ProcessingException<ProxyError> processingException)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, processingException.Message, processingException.StackTrace));
                    result.BusinessException.ErrorCode = StatusCode.InternalProcessingError;
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, exception.Message, exception.StackTrace));
                result.BusinessException.ErrorCode = StatusCode.InternalProcessingError;
            }
            return result;
        }

        public TrackingIdentifierResult AssignTrackingIdentifiers(PaymentInstructionBusinessData paymentInstructionBusinessData, List<string> trackingNumbers)
        {
            var trackingIdentifier = new TrackingIdentifierResult
            {
                ForHeader = trackingNumbers[0],
                ForCredit = trackingNumbers[1],
                ForCheques = new List<PostingChequeTracking>()
            };

            var startIndex = 2;
            foreach (var postingCheque in paymentInstructionBusinessData.PostingCheques)
            {
                trackingIdentifier.ForCheques.Add(new PostingChequeTracking()
                    {
                        Cheque = postingCheque,
                        TrackingId = trackingNumbers[startIndex]
                    }
                );

                startIndex++;
            }
            return trackingIdentifier;
        }

        public PaymentInstructionStoreResult StorePaymentInstruction(PaymentInstructionBusinessData data, TrackingIdentifierResult trackingIdentifierResult)
        {
            var result = new PaymentInstructionStoreResult();
            try
            {
                var account = data.GetEntityAccount();
                var accountNames = data.GetEntityAccountNames();
                account = _paymentInstructionDataAccess.GetOrCreateAccount(account, accountNames, trackingIdentifierResult.ForCredit, data.ChannelType, data.ClientSession.SessionId);

                var session = data.GetEntityClientSession();
                session = _paymentInstructionDataAccess.CreateSession(session, trackingIdentifierResult.ForCredit, data.ChannelType);

                var paymentInstruction = data.GetEntityPaymentInstruction(session.Id, account.Id, trackingIdentifierResult.ForCredit);
                paymentInstruction = _paymentInstructionDataAccess.CreatePaymentInstruction(paymentInstruction);

                var vouchersDictionary = data.GetEntityVouchers(account, paymentInstruction.Id, trackingIdentifierResult, _configuration);
                _paymentInstructionDataAccess.SaveVouchers(vouchersDictionary, trackingIdentifierResult.ForCredit, data.ChannelType, data.ClientSession.SessionId);

                result.PaymentInstructionId = paymentInstruction.Id;

            }
            catch (ProcessingException<PaymentInstructionError> processingException)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, processingException.Message, string.Empty));

                if (processingException.Error == PaymentInstructionError.EntityNotFoundError)
                    result.BusinessException.ErrorCode = StatusCode.EntityNotFound;

                if (processingException.Error == PaymentInstructionError.DatabaseAccessError)
                    result.BusinessException.ErrorCode = StatusCode.DatabaseOrFileAccessError;
            }
            catch (ProcessingException<ProxyError> processingException)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, processingException.Message, processingException.StackTrace));
                result.BusinessException.ErrorCode = StatusCode.InternalProcessingError;
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, exception.Message, exception.StackTrace));
                result.BusinessException.ErrorCode = StatusCode.InternalProcessingError;
            }
            return result;

        }

        public BusinessResult UpdatePaymentInstructionWithShadowPost(long paymentInstructionId, DateTime? shadowPostProcessingDate)
        {
            var result = new BusinessResult();

            try
            {
                // We are not expecting NAB IB includes time in the processing date but will use DateCompare to include time in the logic here.
                 
                var currentDateTime = shadowPostProcessingDate ?? DateTime.Now;

                var startDateTimeRange = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day,
                    _configuration.DipsTransportProcessingTimeRange.StartHour, 0, 0);
                var endDateTimeRange = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day,
                    _configuration.DipsTransportProcessingTimeRange.EndHour, 0, 0);

                //Time is not required to be included in the processing dateTime field in database.
                //The dips transport scheduler will take care about the time range specified in its own config.
                DateTime processingDateTime;

                if (DateTime.Compare(currentDateTime, startDateTimeRange) < 0 || DateTime.Compare(currentDateTime, endDateTimeRange) > 0)
                {
                    processingDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0).AddDays(1).Date;
                }
                else
                {
                    processingDateTime = currentDateTime.Date;
                }
                _paymentInstructionDataAccess.UpdatePaymentInstructionProcessingDate(paymentInstructionId, processingDateTime);
            }
            catch (ProcessingException<PaymentInstructionError> processingException)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, processingException.Message, string.Empty));

                if (processingException.Error == PaymentInstructionError.EntityNotFoundError)
                    result.BusinessException.ErrorCode = StatusCode.EntityNotFound;

                if (processingException.Error == PaymentInstructionError.DatabaseAccessError)
                    result.BusinessException.ErrorCode = StatusCode.DatabaseOrFileAccessError;
            }
            catch (ProcessingException<ProxyError> processingException)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, processingException.Message, processingException.StackTrace));
                result.BusinessException.ErrorCode = StatusCode.InternalProcessingError;
            }
            catch (Exception exception)
            {
                result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException,
                    string.Empty, exception.Message, exception.StackTrace));
                result.BusinessException.ErrorCode = StatusCode.InternalProcessingError;
            }
            return result;
        }
    }
}