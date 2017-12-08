using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using FXA.DPSE.Service.DTO.NabInternetBanking;

namespace FXA.DPSE.NAB.InternetBankingDummy
{
    [ServiceContract]
    public interface IInternetBankingDummyService
    {
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "internetbanking/updateaccountbalance"
        )]
        [OperationContract]
        UpdateAccountBalanceResponse UpdateAccountBalance(UpdateAccountBalanceRequest request);
    }
}