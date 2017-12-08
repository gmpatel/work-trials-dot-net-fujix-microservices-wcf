using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DTO.DipsTransport;

namespace FXA.DPSE.Service.DipsTransport.Common.Converters
{
    public static class DtoToBusinessConverterExtension
    {
        public static DipsTransportBusinessData GetDipsTransportBusinessData(this DipsTransportPayloadRequest request)
        {
            return new DtoToBusinessConverter().GetDipsTransportBusinessDataFromDipsTransportPayloadRequest(request);
        }

        public static DipsTransportBusinessData GetDipsTransportBusinessData(this DipsTransportEodRequest request)
        {
            return new DtoToBusinessConverter().GetDipsTransportBusinessDataFromDipsTransportEodRequest(request);
        }
    }

    public class DtoToBusinessConverter
    {
        public DipsTransportBusinessData GetDipsTransportBusinessDataFromDipsTransportPayloadRequest(DipsTransportPayloadRequest request)
        {
            return new DipsTransportBusinessData
            {
                RequestGuid = request.RequestGuid,
                IpAddressV4 = request.IpAddressV4,
                MessageVersion = request.MessageVersion,
                RequestUtc = request.RequestUtc
            };
        }

        public DipsTransportBusinessData GetDipsTransportBusinessDataFromDipsTransportEodRequest(DipsTransportEodRequest request)
        {
            return new DipsTransportBusinessData
            {
                RequestGuid = request.RequestGuid,
                IpAddressV4 = request.IpAddressV4,
                MessageVersion = request.MessageVersion,
                RequestUtc = request.RequestUtc,
                ReportDate = request.ReportDate
            };
        }
    }
}