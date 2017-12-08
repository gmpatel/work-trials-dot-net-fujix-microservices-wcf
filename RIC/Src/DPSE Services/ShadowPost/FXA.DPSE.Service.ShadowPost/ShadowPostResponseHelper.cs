using System.Net;
using FXA.DPSE.Service.DTO.NabInternetBanking;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.ShadowPost.Common;

namespace FXA.DPSE.Service.ShadowPost
{
    public class ShadowPostResponseHelper
    {
        public static ShadowPostResponse GetBasicShadowPostResponse(ShadowPostRequest request)
        {
            var shadowPostResponse = new ShadowPostResponse
            {
                MessageVersion = request.MessageVersion,
                RequestUtc = request.RequestUtc,
                RequestGuid = request.RequestGuid,
                ChannelType = request.ChannelType,
                TrackingId = request.TrackingId,
                ClientSession = new ClientSession
                {
                    SessionId = request.ClientSession.SessionId,
                    UserId1 = request.ClientSession.UserId1,
                    UserId2 = request.ClientSession.UserId2,
                    IpAddressV4 = request.ClientSession.IpAddressV4,
                    IpAddressV6 = request.ClientSession.IpAddressV6,
                    CaptureDevice = request.ClientSession.CaptureDevice,
                    ClientName = request.ClientSession.ClientName,
                    ClientVersion = request.ClientSession.ClientVersion,
                    DeviceId = request.ClientSession.DeviceId,
                    Os = request.ClientSession.Os,
                    OsVersion = request.ClientSession.OsVersion
                },
                TotalTransactionAmount = request.TotalTransactionAmount,
                ChequeCount = request.ChequeCount,
            };
            return shadowPostResponse;
        }

        public static HttpStatusCode GetHttpStatusCodeByIbResponse(UpdateAccountBalanceResponse facadeResponse, out string dpseStatusCode)
        {
            dpseStatusCode = string.Empty;
            var httpStatusCode = HttpStatusCode.BadRequest;

            switch (facadeResponse.Status.Code)
            {
                case "IB-373":
                    dpseStatusCode = StatusCode.FailedOnAccountTypes;
                    break;
                case "IB-374":
                    dpseStatusCode = StatusCode.FailedOnAccountHolderName;
                    break;
                case "IB-375":
                    dpseStatusCode = StatusCode.FailedOnAccountNotBelongToNin;
                    break;
                case "IB-093":
                    dpseStatusCode = StatusCode.ApiUnavailableDueToGlobalSwitchError;
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
                case "IB-094":
                    dpseStatusCode = StatusCode.ApiErrorDueToDatabaseException;
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
                case "IB-097":
                    dpseStatusCode = StatusCode.ApiUnavailableDueToBacknedError;
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
                case "IB-099":
                    dpseStatusCode = StatusCode.ApiUnavailableDueToRunTimeException;
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
                case "IB-111":
                    dpseStatusCode = StatusCode.UnAuthorizedAccess;
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            return httpStatusCode;
        }
    }
}
