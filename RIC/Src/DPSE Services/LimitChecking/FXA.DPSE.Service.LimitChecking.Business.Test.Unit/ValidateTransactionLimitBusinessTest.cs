using System;
using System.Collections.Generic;
using System.Linq;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.LimitChecking.Business.BusinessEntity;
using FXA.DPSE.Service.LimitChecking.Common;
using FXA.DPSE.Service.LimitChecking.Common.Configuration;
using FXA.DPSE.Service.LimitChecking.Common.Configuration.Elements;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.LimitChecking.Business.Test.Unit
{
    public class ValidateTransactionLimitBusinessTest
    {
        [Theory, MemberData("ExceededLimitPayload")]
        public void PayloadIsNotValidWhenTotalChequeAmountIsExceededThanFixedLimit(ChequePayload chequePayload)
        {
            var configurationMock = new Mock<ILimitCheckingServiceConfiguration>();
            configurationMock.Setup(e => e.TransactionLimit).Returns(new TransactionLimit {Amount = 50000});
            var auditProxyMock = new AuditProxyFake();

            var validateTransactionLimitBusiness = new ValidateTransactionLimitBusiness(configurationMock.Object,
                auditProxyMock);
            var businessResult = validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(chequePayload);

            Assert.True(businessResult.HasException);
            Assert.Equal(businessResult.BusinessException.ErrorCode, StatusCode.LimitCheckExceeded);

        }

        [Theory, MemberData("Payloads")]
        public void ShouldNotAuditWhenTrackingIdNotAvailable(ChequePayload chequePayload)
        {
            var configurationMock = new Mock<ILimitCheckingServiceConfiguration>();
            configurationMock.Setup(e => e.TransactionLimit).Returns(new TransactionLimit {Amount = 50000});
            var auditMock = new Mock<IAuditProxy>();
            auditMock.Setup(e => e.AuditAsync(It.IsAny<AuditProxyRequest>())).Verifiable();

            var validateTransactionLimitBusiness = new ValidateTransactionLimitBusiness(configurationMock.Object,
                auditMock.Object);

            validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(chequePayload);
            auditMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.Never);
        }

        [Theory, MemberData("PayloadsWithTrackingId")]
        public void ShouldAuditByTrackingIdentifierWithAnyValidationResult(ChequePayload chequePayload)
        {
            var configurationMock = new Mock<ILimitCheckingServiceConfiguration>();
            configurationMock.Setup(e => e.TransactionLimit).Returns(new TransactionLimit {Amount = 50000});
            var auditMock = new Mock<IAuditProxy>();
            auditMock.Setup(e => e.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult()).Verifiable();

            var validateTransactionLimitBusiness = new ValidateTransactionLimitBusiness(configurationMock.Object,
                auditMock.Object);

            validateTransactionLimitBusiness.ValidatePayloadTransactionLimit(chequePayload);
            auditMock.Verify(e => e.AuditAsync(It.IsAny<AuditProxyRequest>()), Times.Exactly(1));
        }


        public static IEnumerable<object[]> ExceededLimitPayload
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new ChequePayload
                        {
                            Cheques = new List<DepositCheque> {new DepositCheque() {ChequeAmount = 55000}}.ToArray()
                        }
                    },
                    new object[]
                    {
                        new ChequePayload
                        {
                            Cheques = new List<DepositCheque>
                            {
                                new DepositCheque {ChequeAmount = 450000},
                                new DepositCheque {ChequeAmount = 250000}
                            }.ToArray()
                        }
                    }
                };
            }
        }
        public static IEnumerable<object[]> Payloads
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new ChequePayload
                        {
                            Cheques = new List<DepositCheque>
                            {
                                new DepositCheque {ChequeAmount = 250000},
                                new DepositCheque {ChequeAmount = 250000}
                            }.ToArray()
                        },
                    },
                    new object[]
                    {
                        new ChequePayload
                        {
                            Cheques = new List<DepositCheque>
                            {
                                new DepositCheque {ChequeAmount = 450000},
                                new DepositCheque {ChequeAmount = 250000}
                            }.ToArray()
                        }

                    }
                };
            }
        }
        public static IEnumerable<object[]> PayloadsWithTrackingId
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new ChequePayload
                        {
                            TrackingId = "1100",
                            Cheques = new List<DepositCheque>
                            {
                                new DepositCheque {ChequeAmount = 250000},
                                new DepositCheque {ChequeAmount = 250000}
                            }.ToArray()
                        },
                    },
                    new object[]
                    {
                        new ChequePayload
                        {
                            TrackingId = "1200",
                            Cheques = new List<DepositCheque>
                            {
                                new DepositCheque {ChequeAmount = 450000},
                                new DepositCheque {ChequeAmount = 250000}
                            }.ToArray()
                        }

                    }
                };
            }
        }
    }
}


