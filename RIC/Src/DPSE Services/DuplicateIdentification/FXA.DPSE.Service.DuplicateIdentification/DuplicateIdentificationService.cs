using System.Linq;
using System;
using System.Net;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.DTO.DuplicateIdentification;
using FXA.DPSE.Service.DuplicateIdentification.Common;


namespace FXA.DPSE.Service.DuplicateIdentification
{
    [LoggingBehavior]
    [ErrorBehavior("DPSE-8904")]
    [ValidationBehavior("DPSE-8901")]
    public class DuplicateIdentificationService : DpseServiceBase, IDuplicateIdentificationService
    {
        public DuplicateIdentificationResponse DuplicateIdentification(DuplicateIdentificationRequest duplicateIdentificationRequest)
        {
            var duplicateIdentificationResponse = new DuplicateIdentificationResponse()
            {
                Code = StatusCode.DuplicateIdentificationSuccessful,
                Message = "Duplicate Identification Successful",
                TrackingId = duplicateIdentificationRequest.TrackingId,
            };
            var duplicateIdentificationCheques = duplicateIdentificationRequest.Cheques.Select(cheque => new ChequeResponse()
            {
                SequenceId = cheque.SequenceId, 
                ChequeResponseCode = StatusCode.DuplicateIdentificationSuccessful,
                ChequeResponseDescription = "Duplicate Identification Successful"
            }).ToList();
            duplicateIdentificationResponse.Cheques = duplicateIdentificationCheques.ToArray();
            return DpseResponse(duplicateIdentificationResponse, HttpStatusCode.OK);
        }
    }
}
