using System;
using System.ServiceModel.Activation;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.Service.PaymentValidation;
using FXA.DPSE.Service.PaymentValidation.Core;

namespace FXA.DPSE.NAB.Service.PaymentValidation.Endpoint
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            IContainer container = Bootstrapper.BuildContainer();
            AutofacHostFactory.Container = container;

            RouteTable.Routes.Add(
                new ServiceRoute("dpse",
                    new RestServiceHostFactory<IPaymentValidationService>(),
                    typeof(IPaymentValidationService)));
        }
    }
}