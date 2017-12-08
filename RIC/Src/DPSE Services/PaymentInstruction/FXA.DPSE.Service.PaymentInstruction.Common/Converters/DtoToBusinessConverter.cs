using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DTO.PaymentInstruction;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;

namespace FXA.DPSE.Service.PaymentInstruction.Common.Converters
{
    public static class DtoToBusinessConverterExtensions
    {
        public static PaymentInstructionBusinessData GetPaymentInstructionBusinessData(this PaymentInstructionRequest request)
        {
            return new DtoToBusinessConverter().GetPaymentInstructionBusinessDataFromPaymentInstructionRequest(request);
        }

        public static PaymentInstructionStatusUpdateBusinessData GetPaymentInstructionStatusUpdateBusinessData(this PaymentInstructionStatusUpdateRequest request)
        {
            return new DtoToBusinessConverter().GetPaymentInstructionStatusUpdateBusinessDataFromPaymentInstructionStatusUpdateRequest(request);
        }
    }

    public class DtoToBusinessConverter
    {
        public PaymentInstructionStatusUpdateBusinessData GetPaymentInstructionStatusUpdateBusinessDataFromPaymentInstructionStatusUpdateRequest(PaymentInstructionStatusUpdateRequest request)
        {
            var businessData = new PaymentInstructionStatusUpdateBusinessData
            {
                MessageVersion = request.MessageVersion,
                RequestDateTimeUtc = request.RequestDateTimeUtc,
                Id = request.Id,
                TrackingId = request.TrackingId,
                ChannelType = request.ChannelType,
                Status = request.Status,
                
                ClientSession = new PaymentInstructionSession
                {
                    SessionId = request.ClientSession.SessionId,
                    DeviceId = request.ClientSession.DeviceId,
                    IpAddressV4 = request.ClientSession.IpAddressV4,
                    IpAddressV6 = request.ClientSession.IpAddressV6,
                    CaptureDevice = request.ClientSession.CaptureDevice,
                    ClientName = request.ClientSession.ClientName,
                    ClientVersion = request.ClientSession.ClientVersion,
                    Os = request.ClientSession.Os,
                    OsVersion = request.ClientSession.OsVersion,
                    UserId1 = request.ClientSession.UserId1,
                    UserId2 = request.ClientSession.UserId2
                }
            };

            return businessData;
        }

        public PaymentInstructionBusinessData GetPaymentInstructionBusinessDataFromPaymentInstructionRequest(PaymentInstructionRequest request)
        {
            var businessData = new PaymentInstructionBusinessData
            {
                MessageVersion = request.MessageVersion,
                RequestDateTimeUtc = request.RequestDateTimeUtc,
                Id = request.Id,
                TotalTransactionAmount = request.TotalTransactionAmount,
                ChannelType = request.ChannelType,
                ChequeCount = request.ChequeCount,
                TransactionNarrative = request.TransactionNarrative,
                ClientSession = new PaymentInstructionSession
                {
                    SessionId = request.ClientSession.SessionId,
                    DeviceId = request.ClientSession.DeviceId,
                    IpAddressV4 = request.ClientSession.IpAddressV4,
                    IpAddressV6 = request.ClientSession.IpAddressV6,
                    CaptureDevice = request.ClientSession.CaptureDevice,
                    ClientName = request.ClientSession.ClientName,
                    ClientVersion = request.ClientSession.ClientVersion,
                    Os = request.ClientSession.Os,
                    OsVersion = request.ClientSession.OsVersion,
                    UserId1 = request.ClientSession.UserId1,
                    UserId2 = request.ClientSession.UserId2
                },
                DepositingAccountDetails = new PaymentInstructionAccountDetails
                {

                    Names = new List<PaymentInstructionAccountName>(),
                    BsbCode = request.DepositingAccountDetails.BsbCode,
                    AccountType = request.DepositingAccountDetails.AccountType,
                    AccountNumber = request.DepositingAccountDetails.AccountNumber
                },
                Notifications = new List<PaymentInstructionNotification>(),
                PostingCheques = new List<PaymentInstructionPostingCheque>(),
                NonPostingCheques = new List<PaymentInstructionNonPostingCheque>()
            };

            if (request.DepositingAccountDetails.Names != null && request.DepositingAccountDetails.Names.Count > 0)
            {
                request.DepositingAccountDetails.Names.ToList().ForEach(x =>
                    businessData.DepositingAccountDetails.Names.Add(new PaymentInstructionAccountName
                        {
                            Name = x.Name
                        }
                    )
                );
            }

            if (request.Notifications != null && request.Notifications.Count > 0)
            {
                request.Notifications.ToList().ForEach(x =>
                    businessData.Notifications.Add(new PaymentInstructionNotification
                        {
                            NotificationInfo = x.NotificationInfo,
                            NotificationType = x.NotificationType
                        }
                    )
                );
            }

            if (request.PostingCheques != null && request.PostingCheques.Count > 0)
            {
                request.PostingCheques.ToList().ForEach(x =>
                    businessData.PostingCheques.Add(new PaymentInstructionPostingCheque
                        {
                            ChequeAmount = x.ChequeAmount,
                            Codeline = x.Codeline,
                            SequenceId = x.SequenceId,
                            FrontImage = x.FrontImage,
                            FrontImageSha = x.FrontImageSha,
                            RearImage = x.RearImage,
                            RearImageSha = x.RearImageSha
                        }
                    )
                );
            }

            if (request.NonPostingCheques != null && request.NonPostingCheques.Count > 0)
            {
                request.NonPostingCheques.ToList().ForEach(x =>
                    businessData.NonPostingCheques.Add(new PaymentInstructionNonPostingCheque
                        {
                            ChequeAmount = x.ChequeAmount,
                            Codeline = x.Codeline,
                            SequenceId = x.SequenceId,
                            FrontImage = x.FrontImage,
                            RearImage = x.RearImage,
                        }
                    )
                );
            }

            return businessData;
        }
    }
}
