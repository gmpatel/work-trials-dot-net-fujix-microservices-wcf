using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.Service.PaymentInstruction
{
    public class ResponseHelper
    {
        public static PaymentInstructionFacadeOut<PaymentInstructionResponse> GetBasicPaymentInstructionResponse(PaymentInstructionRequest paymentInstructionRequest)
        {
            var response = new PaymentInstructionResponse
            {
                ChannelType = paymentInstructionRequest.ChannelType,
                ChequeCount = paymentInstructionRequest.ChequeCount,
                ClientSession = paymentInstructionRequest.ClientSession,
                MessageVersion = paymentInstructionRequest.MessageVersion,
                RequestDateTimeUtc = paymentInstructionRequest.RequestDateTimeUtc,
                RequestGuid = paymentInstructionRequest.Id,
                TotalTransactionAmount = paymentInstructionRequest.TotalTransactionAmount
            };
            //response.ResultStatus
            //response.ChequeResponses = paymentInstructionRequest
            //response.TrackingId
            //response.TransactionResponses
            var facadeResult = new PaymentInstructionFacadeOut<PaymentInstructionResponse>();
            facadeResult.Response = response;
            return facadeResult;
        }
    }
}