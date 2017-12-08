using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.DipsTransport.Business;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DTO.DipsTransport;
using FXA.DPSE.Framework.Common.Configuration;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Common.Converters;

namespace FXA.DPSE.Service.DipsTransport
{
    [LoggingBehavior]
    [ErrorBehavior("DPSE-4003")]
    [ValidationBehavior("DPSE-4001")]
    public class DipsTransportService : DpseServiceBase, IDipsTransportService
    {
        private readonly ITransportProcessorFactory _transportProcessorFactory;

        public DipsTransportService(ITransportProcessorFactory transportProcessorFactory)
        {
            this._transportProcessorFactory = transportProcessorFactory;
        }

        public DipsTransportPayloadResponse DipsTransportBatch(DipsTransportPayloadRequest request)
        {
            var response = new DipsTransportPayloadResponse
            {
                RequestGuid = request.RequestGuid
            };

            var businessData = request.GetDipsTransportBusinessData();

            var payloadTransportProcessor = _transportProcessorFactory.GetPayloadTransporter();
            var businessResult = payloadTransportProcessor.Transport(businessData);

            if (businessResult.HasException)
            {
                response.Code = businessResult.BusinessException.ErrorCode;
                response.Message = businessResult.HasInfo ? string.Join(", ", new string[] { businessResult.BusinessInfos().First().Message, businessResult.BusinessException.Message }) : businessResult.BusinessException.Message;

                if (businessResult.BusinessException.ExceptionType != DpseBusinessExceptionType.BusinessRule)
                {
                    return DpseResponse(response, HttpStatusCode.InternalServerError);
                }

                return DpseResponse(response, HttpStatusCode.BadRequest);    
            }

            response.Code = "DPSE-4000";
            response.Message = businessResult.HasInfo ? businessResult.BusinessInfos().First().Message : string.Empty;

            return DpseResponse(response, HttpStatusCode.OK);
        }

        public DipsTransportEodResponse DipsPayloadEod(DipsTransportEodRequest request)
        {
            var response = new DipsTransportEodResponse
            {
                RequestGuid = request.RequestGuid,
            };
            
            DateTime reportDate;

            if(DateTime.TryParseExact(request.ReportDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out reportDate))
            {
                var businessData = request.GetDipsTransportBusinessData();

                var eodTransportProcessor = _transportProcessorFactory.GetEodTransporter();
                var businessResult = eodTransportProcessor.Transport(businessData);

                if (businessResult.HasException)
                {
                    response.Code = businessResult.BusinessException.ErrorCode;
                    response.Message = businessResult.HasInfo ? string.Join(", ", new string[] { businessResult.BusinessInfos().First().Message, businessResult.BusinessException.Message }) : businessResult.BusinessException.Message;

                    if (businessResult.BusinessException.ExceptionType != DpseBusinessExceptionType.BusinessRule)
                    {
                        return DpseResponse(response, HttpStatusCode.InternalServerError);
                    }

                    return DpseResponse(response, HttpStatusCode.BadRequest);
                }

                response.Code = "DPSE-4000";
                response.Message = businessResult.HasInfo ? businessResult.BusinessInfos().First().Message : string.Empty;

                return DpseResponse(response, HttpStatusCode.OK);
            }
            else
            {
                response.Code = "DPSE-4001";
                response.Message = "Invalid ReportDate provided in request. Please provide report date in YYYY-MM-DD format only";
                return DpseResponse(response, HttpStatusCode.BadRequest);    
            }
        }
    }
}