using System;
using System.Collections.Generic;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Business.Test.Unit.Utils;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration.Elements;
using FXA.DPSE.Service.PaymentInstruction.DataAccess;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.PaymentInstruction.Business.Test.Unit
{
    public class PaymentInstructionBusinessTest
    {
        [Fact]
        public void ShouldAssignTrackingIdentifiersOnSpecifiedOrder()
        {
            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            var dataAccessMock = new Mock<IPaymentInstructionDataAccess>();
            var paymentInstructionData = new PaymentInstructionBusinessData()
            {
                PostingCheques = new List<PaymentInstructionPostingCheque>()
                {
                    new PaymentInstructionPostingCheque()
                    {
                        SequenceId = 1,
                        Codeline = "10",
                        ChequeAmount = 2000
                    },
                    new PaymentInstructionPostingCheque()
                    {
                        SequenceId = 2,
                        Codeline = "20",
                        ChequeAmount = 2000
                    }
                }
            };
            var trackingIdentifiers = new List<string> {"1100", "2200", "3300", "4400"};
            var paymentInstructionProcessor = new PaymentInstructionBusiness(configurationMock.Object,
                dataAccessMock.Object);
            var result = paymentInstructionProcessor.AssignTrackingIdentifiers(paymentInstructionData,
                trackingIdentifiers);

            Assert.True(trackingIdentifiers[0] == result.ForHeader,
                "The first tracking identifier is assigned to header");
            Assert.True(trackingIdentifiers[1] == result.ForCredit, "The second identifier is assigned to credit");
            AssertExtensions.AssignedChequesTrackingIdentifier(trackingIdentifiers, result);
        }

        [Theory]
        [InlineData(6, 45, 30)]
        [InlineData(00, 0, 0)]
        [InlineData(23, 25, 59)]
        public void ShouldUpdateWithTomorrowDateWhenShadowPostDateIsOutOfRange(int processingHour, int processingMinute, int processingSecond)
        {
            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.DipsTransportProcessingTimeRange)
                .Returns(new DipsTransportProcessingTimeRangeElement
                {
                    StartHour = 7,
                    EndHour = 23
                });

            var dataAccessMock = new Mock<IPaymentInstructionDataAccess>();
            var expectedProcessingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0).AddDays(1);
            dataAccessMock.Setup(e => e.UpdatePaymentInstructionProcessingDate(It.IsAny<long>(), expectedProcessingDate)).Verifiable();

            var paymentInstructionProcessor = new PaymentInstructionBusiness(configurationMock.Object, dataAccessMock.Object);

            var shadowPostProcessingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, processingHour, processingMinute, processingSecond);
            paymentInstructionProcessor.UpdatePaymentInstructionWithShadowPost(222, shadowPostProcessingDate);
            
            dataAccessMock.Verify(e => e.UpdatePaymentInstructionProcessingDate(It.IsAny<long>(), expectedProcessingDate.Date), Times.Once);
        }

        [Fact]
        public void ShouldUpdateWithShadowPostDateWhenDateIsInSpecifiedHourRange()
        {
            var configurationMock = new Mock<IPaymentInstructionServiceConfiguration>();
            configurationMock.Setup(e => e.DipsTransportProcessingTimeRange)
                .Returns(new DipsTransportProcessingTimeRangeElement
                {
                    StartHour = 7,
                    EndHour = 23
                });

            var shadowPostProcessingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 45);
            var dataAccessMock = new Mock<IPaymentInstructionDataAccess>();
            dataAccessMock.Setup(e => e.UpdatePaymentInstructionProcessingDate(It.IsAny<long>(), shadowPostProcessingDate.Date)).Verifiable();

            var paymentInstructionProcessor = new PaymentInstructionBusiness(configurationMock.Object, dataAccessMock.Object);
            paymentInstructionProcessor.UpdatePaymentInstructionWithShadowPost(222, shadowPostProcessingDate);

            dataAccessMock.Verify(e => e.UpdatePaymentInstructionProcessingDate(It.IsAny<long>(), shadowPostProcessingDate.Date));
        }
    }
}
