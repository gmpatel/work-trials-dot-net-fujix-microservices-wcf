using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FXA.DPSE.Framework.Common.Security.SHA1;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Common.Configuration;
using FXA.DPSE.Service.PaymentInstruction.DataAccess;

namespace FXA.DPSE.Service.PaymentInstruction.Business
{
    public static class Base64StaticData
    {
        private static string HeaderVoucherFrontBase64 { get; set; }
        private static string HeaderVoucherFrontSha { get; set; }
        private static string HeaderVoucherRearBase64 { get; set; }
        private static string HeaderVoucherRearSha { get; set; }

        private static string CreditVoucherFrontBase64 { get; set; }
        private static string CreditVoucherFrontSha { get; set; }
        private static string CreditVoucherRearBase64 { get; set; }
        private static string CreditVoucherRearSha { get; set; }

        public static string GetHeaderVoucherFrontBase64(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(HeaderVoucherFrontBase64))
            {
                HeaderVoucherFrontBase64 =
                    Convert.ToBase64String(
                        File.ReadAllBytes(configuration.PayloadProcessingDetails.HeaderVoucherFrontImagePath));
            }

            return HeaderVoucherFrontBase64;
        }

        public static string GetHeaderVoucherFrontSha(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(HeaderVoucherFrontSha))
            {
                HeaderVoucherFrontSha = HeaderVoucherFrontBase64.GetSHA1String();
            }

            return HeaderVoucherFrontSha;
        }

        public static string GetHeaderVoucherRearBase64(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(HeaderVoucherRearBase64))
            {
                HeaderVoucherRearBase64 =
                    Convert.ToBase64String(
                        File.ReadAllBytes(configuration.PayloadProcessingDetails.HeaderVoucherRearImagePath));
            }

            return HeaderVoucherRearBase64;
        }

        public static string GetHeaderVoucherRearSha(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(HeaderVoucherRearSha))
            {
                HeaderVoucherRearSha = HeaderVoucherRearBase64.GetSHA1String();
            }

            return HeaderVoucherRearSha;
        }


        public static string GetCreditVoucherFrontBase64(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(CreditVoucherFrontBase64))
            {
                CreditVoucherFrontBase64 =
                    Convert.ToBase64String(
                        File.ReadAllBytes(configuration.PayloadProcessingDetails.CreditVoucherFrontImagePath));
            }

            return CreditVoucherFrontBase64;
        }

        public static string GetCreditVoucherFrontSha(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(CreditVoucherFrontSha))
            {
                CreditVoucherFrontSha = CreditVoucherFrontBase64.GetSHA1String();
            }

            return CreditVoucherFrontSha;
        }

        public static string GetCreditVoucherRearBase64(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(CreditVoucherRearBase64))
            {
                CreditVoucherRearBase64 =
                    Convert.ToBase64String(
                        File.ReadAllBytes(configuration.PayloadProcessingDetails.CreditVoucherRearImagePath));
            }

            return CreditVoucherRearBase64;
        }

        public static string GetCreditVoucherRearSha(IPaymentInstructionServiceConfiguration configuration)
        {
            if (string.IsNullOrEmpty(CreditVoucherRearSha))
            {
                CreditVoucherRearSha = CreditVoucherRearBase64.GetSHA1String();
            }

            return CreditVoucherRearSha;
        }

    }

    public static class BusinessToEntityConverterExtensions
    {
        public static DataAccess.PaymentInstruction GetEntityPaymentInstruction(this PaymentInstructionBusinessData data, long clientSessionId, long accountId, string trackingNumber)
        {
            return new BusinessToEntityConverter().GetEntityPaymentInstructionFromPaymentInstructionBusinessData(data, clientSessionId, accountId, trackingNumber);
        }

        public static Account GetEntityAccount(this PaymentInstructionBusinessData data)
        {
            return new BusinessToEntityConverter().GetEntityAccountFromPaymentInstructionBusinessData(data);
        }

        public static IList<string> GetEntityAccountNames(this PaymentInstructionBusinessData data)
        {
            return new BusinessToEntityConverter().GetEntityAccountNamesFromPaymentInstructionBusinessData(data);
        }

        public static ClientSession GetEntityClientSession(this  PaymentInstructionBusinessData data)
        {
            return new BusinessToEntityConverter().GetEntityClientSessionFromPaymentInstructionBusinessData(data);
        }

        public static IDictionary<Voucher, VoucherImage> GetEntityVouchers(this PaymentInstructionBusinessData data, Account account, long paymentInstructionId, TrackingIdentifierResult trackingIdentifierResult, IPaymentInstructionServiceConfiguration configuration)
        {
            return new BusinessToEntityConverter().GetEntityVouchersFromPaymentInstructionBusinessData(data, account, paymentInstructionId, trackingIdentifierResult, configuration);
        }

        public static DataAccess.PaymentInstruction GetEntityPaymentInstruction(this PaymentInstructionStatusUpdateBusinessData data)
        {
            return new BusinessToEntityConverter().GetEntityPaymentInstructionFromPaymentInstructionStatusUpdateBusinessData(data);
        }
    }

    public class BusinessToEntityConverter
    {
        public DataAccess.PaymentInstruction GetEntityPaymentInstructionFromPaymentInstructionStatusUpdateBusinessData(PaymentInstructionStatusUpdateBusinessData data)
        {
            if (data != null)
            {
                return new DataAccess.PaymentInstruction
                {
                    ChannelType = data.ChannelType,
                    TrackingId = data.TrackingId
                };
            }

            return null;
        }

        public IDictionary<Voucher, VoucherImage> GetEntityVouchersFromPaymentInstructionBusinessData(PaymentInstructionBusinessData data, Account account, 
            long paymentInstructionId, TrackingIdentifierResult trackingIdentifierResult , IPaymentInstructionServiceConfiguration configuration)
        {
            var result = new Dictionary<Voucher, VoucherImage>();

            if (data != null && data.PostingCheques != null && data.PostingCheques.Count > 0)
            {
                var header = new Voucher
                {
                    PaymentInstructionId = paymentInstructionId,
                    TrackingId = trackingIdentifierResult.ForHeader,
                    SequenceId = 0,
                    VoucherType = configuration.PayloadVoucherType.Header,
                    TransactionCode = configuration.PayloadTransactionCode.Header,
                    AuxDom = string.Empty,
                    ProcessingDateTime = DateTime.UtcNow,
                    BSB = configuration.PayloadBsbNumber.Header,
                    AccountNumber = configuration.PayloadAccountNumber.Header,
                    AmountInCents = 0,
                    IsNonPostingCheque = false
                };

                var headerImages = new VoucherImage
                {
                    FrontImage = Base64StaticData.GetHeaderVoucherFrontBase64(configuration),
                    FrontImageSHA = Base64StaticData.GetHeaderVoucherFrontSha(configuration),
                    RearImage = Base64StaticData.GetHeaderVoucherFrontBase64(configuration),
                    RearImageSHA = Base64StaticData.GetHeaderVoucherFrontSha(configuration)
                };

                result.Add(header, headerImages);

                var creditAmount = 0;

                for (var i = 0; i < data.PostingCheques.Count; i++)
                {
                    var voucher = new Voucher
                    {
                        PaymentInstructionId = paymentInstructionId,
                        TrackingId = trackingIdentifierResult.ForCheques.First(e=>e.Cheque.Codeline == data.PostingCheques[i].Codeline).TrackingId,
                        SequenceId = data.PostingCheques[i].SequenceId,
                        VoucherType = configuration.PayloadVoucherType.Debit,
                        TransactionCode = configuration.PayloadTransactionCode.Debit,
                        AuxDom = data.PostingCheques[i].Codeline,
                        ProcessingDateTime = DateTime.UtcNow,
                        BSB = account.BSBCode,
                        AccountNumber = account.AccountNumber,
                        AmountInCents = data.PostingCheques[i].ChequeAmount,
                        IsNonPostingCheque = false
                    };

                    var voucherImage = new VoucherImage
                    {
                        FrontImage = data.PostingCheques[i].FrontImage,
                        FrontImageSHA = data.PostingCheques[i].FrontImageSha,
                        RearImage = data.PostingCheques[i].RearImage,
                        RearImageSHA = data.PostingCheques[i].RearImageSha
                    };

                    creditAmount = creditAmount + voucher.AmountInCents;

                    result.Add(voucher, voucherImage);
                }

                var credit = new Voucher
                {
                    PaymentInstructionId = paymentInstructionId,
                    TrackingId = trackingIdentifierResult.ForCredit,
                    SequenceId = 0,
                    VoucherType = configuration.PayloadVoucherType.Credit,
                    TransactionCode = configuration.PayloadTransactionCode.Credit,
                    AuxDom = string.Empty,
                    ProcessingDateTime = DateTime.UtcNow,
                    BSB = configuration.PayloadBsbNumber.Credit,
                    AccountNumber = account.AccountNumber,
                    AmountInCents = creditAmount,
                    IsNonPostingCheque = false
                };

                var creditImages = new VoucherImage
                {
                    FrontImage = Base64StaticData.GetCreditVoucherFrontBase64(configuration),
                    FrontImageSHA = Base64StaticData.GetCreditVoucherFrontSha(configuration),
                    RearImage = Base64StaticData.GetCreditVoucherFrontBase64(configuration),
                    RearImageSHA = Base64StaticData.GetCreditVoucherFrontSha(configuration)
                };

                for (var i = 0; i < data.NonPostingCheques.Count; i++)
                {
                    var voucher = new Voucher
                    {
                        PaymentInstructionId = paymentInstructionId,
                        TrackingId = string.Empty,
                        SequenceId = data.NonPostingCheques[i].SequenceId,
                        VoucherType = configuration.PayloadVoucherType.Debit,
                        TransactionCode = configuration.PayloadTransactionCode.Debit,
                        AuxDom = data.NonPostingCheques[i].Codeline,
                        ProcessingDateTime = DateTime.UtcNow,
                        BSB = account.BSBCode,
                        AccountNumber = account.AccountNumber,
                        AmountInCents = data.NonPostingCheques[i].ChequeAmount,
                        IsNonPostingCheque = true
                    };

                    var voucherImage = new VoucherImage
                    {
                        FrontImage = data.NonPostingCheques[i].FrontImage,
                        FrontImageSHA = string.Empty,
                        RearImage = data.NonPostingCheques[i].RearImage,
                        RearImageSHA = string.Empty
                    };

                    creditAmount = creditAmount + voucher.AmountInCents;

                    result.Add(voucher, voucherImage);
                }

                result.Add(credit, creditImages);
            }

            return result;
        }

        public ClientSession GetEntityClientSessionFromPaymentInstructionBusinessData( PaymentInstructionBusinessData data)
        {
            if (data != null)
            {
                return new ClientSession
                {
                    SessionId = data.ClientSession.SessionId,
                    UserId1 = data.ClientSession.UserId1,
                    UserId2 = data.ClientSession.UserId2,
                    DeviceId = data.ClientSession.DeviceId,
                    IpAddressV4 = data.ClientSession.IpAddressV4,
                    IpAddressV6 = data.ClientSession.IpAddressV6,
                    ClientName = data.ClientSession.ClientName,
                    ClientVersion = data.ClientSession.ClientVersion,
                    OS = data.ClientSession.Os,
                    OSVersion = data.ClientSession.OsVersion,
                    CaptureDevice = data.ClientSession.CaptureDevice
                };
            }

            return null;
        }

        public DataAccess.PaymentInstruction GetEntityPaymentInstructionFromPaymentInstructionBusinessData(PaymentInstructionBusinessData data, long clientSessionId, long accountId, string trackingNumber)
        {
            if (data != null)
            {
                return new DataAccess.PaymentInstruction
                {
                    TotalTransactionAmountInCents = data.TotalTransactionAmount,
                    ChannelType = data.ChannelType,
                    ChequeCount = data.ChequeCount,
                    TransactionNarrative = data.TransactionNarrative,
                    TrackingId = trackingNumber,
                    ClientSessionId = clientSessionId,
                    AccountId = accountId,
                    CreatedDateTime = DateTime.UtcNow,
                    Status = "PENDING"
                };
            }

            return null;
        }

        public Account GetEntityAccountFromPaymentInstructionBusinessData(PaymentInstructionBusinessData data)
        {
            if (data != null)
            {
                return new Account
                {
                    AccountNumber = data.DepositingAccountDetails.AccountNumber,
                    BSBCode = data.DepositingAccountDetails.BsbCode,
                    AccountType = data.DepositingAccountDetails.AccountType
                };
            }

            return null;
        }

        public IList<string> GetEntityAccountNamesFromPaymentInstructionBusinessData(PaymentInstructionBusinessData data)
        {
            var result = new List<string>();
                
            if (data != null && data.DepositingAccountDetails.Names != null && data.DepositingAccountDetails.Names.Count > 0)
            {
                data.DepositingAccountDetails.Names.ToList().ForEach(x => result.Add(x.Name));
            }

            return result;
        }
    }
}