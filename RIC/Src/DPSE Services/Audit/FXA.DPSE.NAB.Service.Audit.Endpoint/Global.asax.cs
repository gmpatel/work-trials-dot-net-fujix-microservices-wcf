using System;
using System.ServiceModel.Activation;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.Audit;

namespace FXA.DPSE.NAB.Service.Audit.Endpoint
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = Bootstrapper.BuildContainer();
            AutofacHostFactory.Container = container;

            RouteTable.Routes.Add(
                new ServiceRoute(Routes.DpseRoute,
                    new RestServiceHostFactory<IAuditService>(),
                    typeof(IAuditService)));

            RouteTable.Routes.Add(
                new ServiceRoute(Routes.RicCoreRoute,
                    new RestServiceHostFactory<IAuditService>(),
                    typeof(IAuditService)));
        }
    }
}