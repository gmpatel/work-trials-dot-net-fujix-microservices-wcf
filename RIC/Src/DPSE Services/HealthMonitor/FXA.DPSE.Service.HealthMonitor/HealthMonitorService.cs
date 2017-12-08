using System.Net;
using FXA.DPSE.Framework.Common.RESTClient;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.DTO.HealthMonitor;
using FXA.DPSE.Service.HealthMonitor.Configuration;

namespace FXA.DPSE.Service.HealthMonitor
{
    [ErrorBehavior]
    [ValidationBehavior]
    public class HealthMonitorService : IHealthMonitorService
    {
        private readonly ICustomConfig _config;

        public HealthMonitorService(ICustomConfig config)
        {
            this._config = config;
        }

        public HealthMonitorPostResponse Post(HealthMonitorPostRequest request)
        {
            foreach (var endPoint in this._config.EndPoints)
            {
                var response = HttpClientExtensions.GetSync<string>(endPoint.Url);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // Report To Logging Service OR Do Whatever Require
                }
            }

            return new HealthMonitorPostResponse { Id = request.Id };
        }
    }
}