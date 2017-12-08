using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Autofac.Integration.Wcf;
using FXA.DPSE.NAB.InternetBankingDummy.Endpoint.AppStart;

namespace FXA.DPSE.NAB.InternetBankingDummy.Endpoint
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacHostFactory.Container = Bootstrapper.Container;
        }
    }
}