//using System;
//using System.Collections.Generic;
//using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
//using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
//using FXA.DPSE.Service.DTO.Audit;
//using FXA.DPSE.Service.LimitChecking.Business.BusinessEntity;
//using FXA.DPSE.Service.LimitChecking.Common.Configuration;
//using FXA.DPSE.Service.LimitChecking.Common.Configuration.Elements;
//using FXA.DPSE.Service.PaymentValidation.Common.Configuration;
//using FXA.DPSE.Service.PaymentValidation.Common.Configuration.Elements;
//using Moq;
//using Xunit;
//using Xunit.Sdk;

//namespace FXA.DPSE.Service.LimitChecking.Business.Test.Unit
//{
//    public class LimiCheckingServiceConfigurationFake : IPaymentValidationServiceConfiguration
//    {
//        private int _amount;

//        public LimiCheckingServiceConfigurationFake(int amount)
//        {
//            _amount = amount;
//        }

//        public TransactionLimit TransactionLimit
//        {
//            get
//            {
//                TransactionLimit transactionLimit = new TransactionLimit
//                {
//                    Amount = _amount
//                };
//                return transactionLimit;
//            }
//        }

//        public PaymentValidationServicePath PaymentValidationService
//        {
//            get
//            {
//                PaymentValidationServicePath path = new PaymentValidationServicePath(){ Path = ""};
//                return path;
//            }
//        }
//    }

//    public class ValidateTransactionLimitBusinessTest : IDisposable
//    {
//        private IPaymentValidationServiceConfiguration _paymentValidationServiceConfiguration;
//        private IPaymentValidationBusiness _validateTransactionLimitBusiness;

//        private Mock<ILoggingProxy> _mockLoggingProxy;
//        private Mock<IAuditProxy> _mockAuditProxy;

//        public ValidateTransactionLimitBusinessTest()
//        {
//            _mockLoggingProxy = new Mock<ILoggingProxy>();

//            _mockLoggingProxy.Setup(
//                mock =>
//                    mock.LogEventAsync(
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<Guid>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>()));

//            _mockAuditProxy = new Mock<IAuditProxy>();

//            _mockAuditProxy.Setup(
//                mock => mock.AuditAsync(It.IsAny<AuditRequest>()));
//        }

//        private enum PayloadTestType
//        {
//            LimitNotExceeded,
//            LimitExceeded
//        }

//        private ChequePayload CreateChequePayloadLimitNotExceeded(PayloadTestType type)
//        {
//            if (type == PayloadTestType.LimitNotExceeded)
//            {
//                int limitCheck = 10000;

//                //config setting with amount
//                _paymentValidationServiceConfiguration =
//                    new LimiCheckingServiceConfigurationFake(limitCheck);    
//            }

//            if (type == PayloadTestType.LimitExceeded)   
//            {
//                int limitCheck = 4000;

//                //config setting with amount
//                _paymentValidationServiceConfiguration =
//                    new LimiCheckingServiceConfigurationFake(limitCheck);
//            }

//            ChequePayload payload = new ChequePayload();

//            payload.TrackingId = "1234";
//            payload.ChannelType = "MMC";
//            payload.SessionId = Guid.NewGuid();

//            List<DepositCheque> cheques = new List<DepositCheque>()
//            {
//                new DepositCheque(1000,"1234"),
//                new DepositCheque(2000,"12345"),
//                new DepositCheque(2000,"12345")
//            };

//            payload.Cheques = cheques.ToArray();

//            return payload;
//        }

//        [Fact]
//        public void verify_audit_interaction_when_limit_is_not_exceeded()
//        {
//            ChequePayload payload = 
//                CreateChequePayloadLimitNotExceeded(PayloadTestType.LimitNotExceeded);

//            //instantiating the service
//            _validateTransactionLimitBusiness =
//                new paymentValidationBusiness(
//                    _paymentValidationServiceConfiguration,
//                    _mockLoggingProxy.Object,
//                    _mockAuditProxy.Object);

//            PaymentValidationResult validation = 
//                _validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(payload);
//            _mockAuditProxy.Verify(proxy => proxy.AuditAsync(It.IsAny<AuditRequest>()));
//            Assert.Equal(validation.HasException, false);
//        }

//        [Fact]
//        public void verify_validate_payload_transaction_limit_is_exceeded()
//        {
//            ChequePayload payload = 
//                CreateChequePayloadLimitNotExceeded(PayloadTestType.LimitExceeded);

//            //instantiating the service
//            _validateTransactionLimitBusiness =
//                new paymentValidationBusiness(
//                    _paymentValidationServiceConfiguration,
//                    _mockLoggingProxy.Object,
//                    _mockAuditProxy.Object);

//            PaymentValidationResult validate = 
//                _validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(payload);

//            Assert.Equal(validate.HasException, true);
//        }

//        [Fact]
//        public void verify_audit_interaction_when_limit_is_exceeded()
//        {
//            ChequePayload payload =
//                CreateChequePayloadLimitNotExceeded(PayloadTestType.LimitNotExceeded);

//            //instantiating the service
//            _validateTransactionLimitBusiness =
//                new paymentValidationBusiness(
//                    _paymentValidationServiceConfiguration,
//                    _mockLoggingProxy.Object,
//                    _mockAuditProxy.Object);

//            PaymentValidationResult validation =
//                _validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(payload);

//            _mockAuditProxy.Verify(proxy => proxy.AuditAsync(It.IsAny<AuditRequest>()));
//            Assert.Equal(validation.HasException, true);
//        }

//        [Fact]
//        public void verify_logging_interaction_when_audit_failed()
//        {
//            ChequePayload payload =
//                CreateChequePayloadLimitNotExceeded(PayloadTestType.LimitNotExceeded);

//            _mockAuditProxy = new Mock<IAuditProxy>();

//            _mockAuditProxy.Setup(proxy =>
//                proxy.AuditAsync(It.IsAny<AuditRequest>())
//                ).Throws<Exception>();

//            _mockLoggingProxy = new Mock<ILoggingProxy>();

//            _mockLoggingProxy.Setup(
//                mock =>
//                    mock.LogEventAsync(
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>(),
//                        It.IsAny<Guid>(),
//                        It.IsAny<string>(),
//                        It.IsAny<string>()))
//                        .Returns(new LoggingBusinessResult());

//            //instantiating the service
//            _validateTransactionLimitBusiness =
//                new paymentValidationBusiness(
//                    _paymentValidationServiceConfiguration,
//                    _mockLoggingProxy.Object,
//                    _mockAuditProxy.Object);

//            PaymentValidationResult validation =
//                _validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(payload);

//            _mockLoggingProxy.Verify(proxy => proxy.LogEventAsync(
//                It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<string>(),
//                It.IsAny<Guid>(),
//                It.IsAny<string>(),
//                It.IsAny<string>()));
            
//        }

//        public void Dispose()
//        {
//        }
//    }
//}

