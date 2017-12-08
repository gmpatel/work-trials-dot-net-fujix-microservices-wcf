using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.DuplicateIdentification;

namespace FXA.DPSE.NAB.Service.DuplicateIdentification.Endpoint
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacHostFactory.Container = Bootstrapper.BuildContainer();

            RouteTable.Routes.Add(
               new ServiceRoute("dpse",
                   new RestServiceHostFactory<IDuplicateIdentificationService>(),
                   typeof(IDuplicateIdentificationService)));
        }
    }
}