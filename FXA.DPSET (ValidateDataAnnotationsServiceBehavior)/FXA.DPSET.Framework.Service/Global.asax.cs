using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Autofac.Integration.Wcf;
using FXA.DPSET.Framework.Service.Injection;
using FXA.DPSET.Framework.Service.Library;

namespace FXA.DPSET.Framework.Service
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
            RouteTable.Routes.Add(new ServiceRoute("healthmonitorservice", new AutofacServiceHostFactory(), typeof(IHealthMonitorService)));
            RouteTable.Routes.Add(new ServiceRoute("healthmonitor", new AutofacServiceHostFactory(), typeof(IHealthMonitorService)));
        }
    }
}