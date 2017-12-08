using System.Net;
using System.ServiceModel.Web;

namespace FXA.DPSE.Framework.Service.WCF
{
    public class DpseServiceBase
    {
        protected T DpseResponse<T>(T serviceResponse, HttpStatusCode statusCode)
        {
            if (WebOperationContext.Current != null)
            {
                var response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = statusCode;
            }

            return serviceResponse;
        }
    }
}