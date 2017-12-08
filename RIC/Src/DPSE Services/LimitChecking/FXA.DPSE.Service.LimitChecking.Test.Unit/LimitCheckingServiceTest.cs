using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.LimitChecking;
using FXA.DPSE.Service.LimitChecking.Business;
using FXA.DPSE.Service.LimitChecking.Business.BusinessEntity;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.LimitChecking.Test.Unit
{
    public class LimitCheckingServiceTest
    {
        //TODO: https://wcfmock.codeplex.com/ > Nuget package for WebOperationContext
        //[Fact] 
        //public void ShouldSendErrorLogWithLoggingServiceProxyOnUnhandeledException()
        //{
        //    var validationBusinessMock = new Mock<IValidateTransactionLimitBusiness>();
        //    validationBusinessMock.Setup(e => e.ValidatePayloadTransactionLimit(It.IsAny<ChequePayload>()))
        //        .Throws(new Exception());
        //    var loggingProxyMock = new Mock<ILoggingProxy>();
        //    loggingProxyMock.Setup(e=>e.LogEventAsync(It.IsAny<string>(), It.IsAny<string>(), 
        //        It.IsAny<string>(), It.IsAny<string>()
        //        , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new BusinessResult()).Verifiable();
           
        //    var response = new LimitCheckingService(validationBusinessMock.Object, loggingProxyMock.Object).Limitchecking(new TransactionLimitRequest());

        //    loggingProxyMock.Verify(
        //        e => e.LogEventAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
        //            , It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //            It.IsAny<string>()), Times.Once);

        //}
    }
}
