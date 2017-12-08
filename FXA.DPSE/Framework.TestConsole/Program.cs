using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Common;
using FXA.DPSE.Framework.Common.Extensions;
using FXA.DPSE.Framework.Model.DipsTransport;

namespace FXA.DPSE.Framework.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();

            var request = new DipsTransportRequest { Id = Guid.NewGuid(), Message = "Process Dips Files Transfer" };
            var client = new RESTClient();

            Console.WriteLine("Sending Request = {0}", request.ToJson());
            
            var response = client
                .TryPost<DipsTransportRequest, DipsTransportResponse>("http://localhost:61822/dipstransport/post", request)
                .Result;

            Console.WriteLine("Response Received = {0}", response.ToJson());
            Console.ReadKey();
        }
    }
}