using System;
using System.ServiceModel.Activation;
using System.Web.Routing;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.Logging;

namespace FXA.DPSE.NAB.Service.Logging.Endpoint
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = Bootstrapper.BuildContainer();
            AutofacHostFactory.Container = container;

            RouteTable.Routes.Add(new ServiceRoute(Routes.LoggingService, new RestServiceHostFactory<ILoggingService>(), typeof(ILoggingService)));
        }
    }
}