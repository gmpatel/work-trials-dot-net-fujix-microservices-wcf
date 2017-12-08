using FXA.DPSE.Service.DipsPayload.Business.Entity;
using FXA.DPSE.Service.DTO.DipsPayload;

namespace FXA.DPSE.Service.DipsPayload.Common.Converters
{
    public static class DtoToBusinessConverterExtensions
    {
        public static DipsPayloadBusinessData GetDipsPayloadBusinessData(this DipsPayloadBatchRequest request)
        {
            return new DtoToBusinessConverter().GetDipsPayloadBusinessDataFromDipsPayloadBatchRequest(request);
        }

        public static DipsPayloadSingleBusinessData GetDipsPayloadSingleBusinessData(this DipsPayloadSingleBatchRequest request)
        {
            return new DtoToBusinessConverter().GetDipsPayloadSingleBusinessDataFromDipsPayloadSingleBatchRequest(request);
        }
    }

    public class DtoToBusinessConverter
    {
        public DipsPayloadBusinessData GetDipsPayloadBusinessDataFromDipsPayloadBatchRequest(DipsPayloadBatchRequest request)
        {
            return new DipsPayloadBusinessData
            {
                MessageVersion = request.MessageVersion,
                ClientName = request.ClientName,
                RequestDateTimeUtc = request.RequestDateTimeUtc,
                IpAddressV4 = request.IpAddressV4
            };
        }

        public DipsPayloadSingleBusinessData GetDipsPayloadSingleBusinessDataFromDipsPayloadSingleBatchRequest(DipsPayloadSingleBatchRequest request)
        {
            return new DipsPayloadSingleBusinessData
            {
                MessageVersion = request.MessageVersion,
                ClientName = request.ClientName,
                RequestDateTimeUtc = request.RequestDateTimeUtc,
                IpAddressV4 = request.IpAddressV4,
                PaymentInstructionId = request.PaymentInstructionId
            };
        }
    }
}