using System.Net;
using FXA.DPSE.Framework.Common;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.DipsPayload.Business.Core;
using FXA.DPSE.Service.DipsPayload.Common;
using FXA.DPSE.Service.DTO.DipsPayload;

namespace FXA.DPSE.Service.DipsPayload
{
    [ErrorBehavior("DPSE-6003")]
    [ValidationBehavior("DPSE-6001")]
    [LoggingBehavior]
    public class DipsPayloadService : DpseServiceBase, IDipsPayloadService
    {
        private readonly IPayloadBatchCreator _payloadProcessor;

        public DipsPayloadService(IPayloadBatchCreator payloadProcessor)
        {
            _payloadProcessor = payloadProcessor;
        }

        public DipsPayloadBatchResponse DipsPayloadBatch(DipsPayloadBatchRequest request)
        {
            var businessResult = _payloadProcessor.GeneratePayload();
            var response = new DipsPayloadBatchResponse();
            
            if (businessResult.HasException)
            {
                response.Code = businessResult.BusinessException.ErrorCode;
                response.Message = businessResult.BusinessException.Message;
                response.ResultStatus = "Fail";

                switch (businessResult.BusinessException.ErrorCode)
                {
                    case StatusCode.InternalProcessingError:
                    case StatusCode.DatabaseOrFileAccessError:
                        return DpseResponse(response, HttpStatusCode.InternalServerError);
                    case StatusCode.PaymentInstructionNotFound:
                        return DpseResponse(response, HttpStatusCode.NotFound);
                }
            }
            response.Code = StatusCode.DipsPayloadCreationSuccessful;
            response.Message = (businessResult.ProcessedBatchCount > 0) ? string.Format("{0} Dips Payload created successfully.", businessResult.ProcessedBatchCount) : "No Dips Payload found to be processed.";
            response.ResultStatus = "Success";
            return DpseResponse(response, HttpStatusCode.OK);
        }

        public DipsPayloadSingleBatchResponse DipsPayloadSingleBatch(DipsPayloadSingleBatchRequest request)
        {
            var response = new DipsPayloadSingleBatchResponse() {PaymentInstructionId = request.PaymentInstructionId};
            var businessResult = _payloadProcessor.GeneratePayload(request.PaymentInstructionId);

            if (businessResult.HasException)
            {
                response.Code = businessResult.BusinessException.ErrorCode;
                response.Message = businessResult.BusinessException.Message;
                response.ResultStatus = "Fail";

                switch (businessResult.BusinessException.ErrorCode)
                {
                    case StatusCode.InternalProcessingError:
                    case StatusCode.DatabaseOrFileAccessError:
                        return DpseResponse(response, HttpStatusCode.InternalServerError);
                    case StatusCode.PaymentInstructionNotFound:
                        return DpseResponse(response, HttpStatusCode.NotFound);
                }
            }
            response.Code = StatusCode.DipsPayloadCreationSuccessful;
            response.Message = "Dips Payload creation successful.";
            response.ResultStatus = "Success";
            response.PaymentInstructionId = request.PaymentInstructionId;
            return DpseResponse(response, HttpStatusCode.OK);
        }
    }
}