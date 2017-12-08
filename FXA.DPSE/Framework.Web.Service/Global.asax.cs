using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Web.Service.Injection;
using FXA.DPSE.Framework.Web.Service.Library.DipsTransport;

namespace FXA.DPSE.Framework.Web.Service
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacHostFactory.Container = AutofacBootstrapper.Container;
            ConfigureRoutes();
        }

        private static void ConfigureRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("dipstransportservice", new AutofacServiceHostFactory(), typeof(IDipsTransportService)));
            RouteTable.Routes.Add(new ServiceRoute("dipstransport", new AutofacServiceHostFactory(), typeof(IDipsTransportService)));
        }
    }
}