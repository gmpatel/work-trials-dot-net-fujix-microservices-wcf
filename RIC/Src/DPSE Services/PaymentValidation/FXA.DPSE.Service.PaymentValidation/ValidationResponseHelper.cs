using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DTO.PaymentValidation;

namespace FXA.DPSE.Service.PaymentValidation
{
    public static class ValidationResponseHelper
    {
        public static PaymentValidationResponse GetBasicValidationResponse(PaymentValidationRequest validationRequest)
        {
            var paymentValidationResponse = new PaymentValidationResponse
            {
                MessageVersion = validationRequest.MessageVersion,
                RequestUtc = validationRequest.RequestUtc,
                RequestGuid = validationRequest.RequestGuid,
                TrackingId = validationRequest.TrackingId,
                ChannelType = validationRequest.ChannelType,
                ChequeCount = validationRequest.ChequeCount,
                ClientSession = validationRequest.ClientSession,
            };
            return paymentValidationResponse;
        }
    }
}
