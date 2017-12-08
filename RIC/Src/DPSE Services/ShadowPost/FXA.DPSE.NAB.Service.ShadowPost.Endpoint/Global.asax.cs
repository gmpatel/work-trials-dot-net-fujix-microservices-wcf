using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Wcf;
using FXA.DPSE.Framework.Service.WCF.Host;
using FXA.DPSE.NAB.Service.ShadowPost.Endpoint.AppStart;
using FXA.DPSE.Service.ShadowPost;

namespace FXA.DPSE.NAB.Service.ShadowPost.Endpoint
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacHostFactory.Container = Bootstrapper.Container;
        }
    }
}