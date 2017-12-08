using System;
using System.Net;
using System.ServiceModel.Dispatcher;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Ping 
{
    internal class PingInvoker : IOperationInvoker 
    {
        public object[] AllocateInputs() 
        {
            return new object[0];
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs) 
        {
            outputs = new object[0];

            var domain = Environment.UserDomainName;
            var machine = Environment.MachineName;
            var ip = (Dns.GetHostAddresses(Environment.MachineName) == null
                ? "127.0.0.1"
                : Dns.GetHostAddresses(Environment.MachineName)[0].ToString());
            var time = DateTime.Now;

            return string.Format("{0} ({1}) : {2} : {3} (UTC) : {4} (Local)", machine, domain, ip, time.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous 
        {
            get { return true; }
        }
    }
}