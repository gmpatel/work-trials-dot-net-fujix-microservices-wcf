using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.NAB.Service.HealthMonitor.Endpoint.Injection;

namespace FXA.DPSE.NAB.Service.HealthMonitor.Endpoint
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacHostFactory.Container = Bootstrapper.Container;
        }
    }
}