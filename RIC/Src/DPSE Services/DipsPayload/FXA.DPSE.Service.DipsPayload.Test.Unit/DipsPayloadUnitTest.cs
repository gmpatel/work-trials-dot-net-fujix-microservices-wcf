using FXA.DPSE.Service.DipsPayload.Business.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Business;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DipsPayload.Business;
using FXA.DPSE.Service.DipsPayload.Business.Entity;
using FXA.DPSE.Service.DipsPayload.Common.Configuration;
using FXA.DPSE.Service.DipsPayload.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsPayload.DataAccess;
using FXA.DPSE.Service.DTO.DipsPayload;
using Xunit;

namespace FXA.DPSE.Service.DipsPayload.Test.Unit
{
    public class DipsPayloadUnitTest
    {
        private static DipsPayloadBusinessData GetDipsPayloadBusinessData(DipsPayloadBatchRequest request = null)
        {
            return new DipsPayloadBusinessData
            {
                RequestDateTimeUtc = request == null ? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T") : request.RequestDateTimeUtc,
                IpAddressV4 = request == null ? "127.0.0.1" : request.IpAddressV4,
                MessageVersion = request == null ? "v2.0" : request.MessageVersion,
                ClientName = request == null ? "Mobile iPhone 6S" : request.ClientName
            };
        }

        private static IAuditProxy GetAuditProxyMock()
        {
            var proxy = new Mock<IAuditProxy>();

            proxy.Setup(x => x.AuditAsync(It.IsAny<AuditProxyRequest>())).Returns(new BusinessResult());
            proxy.Setup(x => x.AuditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new BusinessResult());
            proxy.Setup(x => x.AuditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new BusinessResult());

            return proxy.Object;
        }

        private static ILoggingProxy GetLoggingProxyMock()
        {
            var proxy = new Mock<ILoggingProxy>();

            proxy.Setup(x => x.LogEventAsync(It.IsAny<LoggingProxyRequest>())).Returns(new BusinessResult());
            proxy.Setup(x => x.LogEventAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new BusinessResult());

            return proxy.Object;
        }

        private static IFileSystem GetFileSystemMock(bool good = true)
        {
            var obj = new Mock<IFileSystem>();

            if (good)
            {
                obj.Setup(x => x.FileExists(It.IsAny<FileInfo>())).Returns(true);
                obj.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
                obj.Setup(x => x.DirectoryExists(It.IsAny<DirectoryInfo>())).Returns(true);
                obj.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
            }
            else
            {
                obj.Setup(x => x.FileExists(It.IsAny<FileInfo>())).Returns(false);
                obj.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
                obj.Setup(x => x.DirectoryExists(It.IsAny<DirectoryInfo>())).Returns(false);
                obj.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);
             }

            return obj.Object;
        }

        private static IDipsPayloadImagesCreator GetDipsPayloadImagesCreatorMock(bool good = true)
        {
            if (good)
            {
                var imaegCreator = new Mock<IDipsPayloadImagesCreator>();
                imaegCreator.Setup(x => x.GeneratePayloadImages(It.IsAny<PaymentInstruction>(), It.IsAny<DirectoryInfo>())).Returns(true);
                return imaegCreator.Object;                
            }
            else
            {
                var imaegCreator = new Mock<IDipsPayloadImagesCreator>();
                imaegCreator.Setup(x => x.GeneratePayloadImages(It.IsAny<PaymentInstruction>(), It.IsAny<DirectoryInfo>())).Returns(false);
                return imaegCreator.Object;                                
            }
        }

        private static IPaymentInstructionDataAccess GetPaymentInstructionDataAccessMock(int paymentInstructionsToBeGenerated = 0, bool paymentInstructionByIdAvailable = true)
        {
            var dataAccess = new Mock<IPaymentInstructionDataAccess>();



            dataAccess.Setup(x => x.GetPaymentInstruction(It.IsAny<long>())).Returns((long id) =>
            {
                if (!paymentInstructionByIdAvailable)
                {
                    return null;
                }

                return new PaymentInstruction
                {
                    Id = id,
                    TotalTransactionAmountInCents = 33600,
                    ChannelType = "AFS-MIB",
                    ChequeCount = 3,
                    TransactionNarrative = "XXXXX XXX XXX",
                    TrackingId = "NAB20151102101000458",
                    ClientSessionId = 1,
                    AccountId = 1,
                    ProcessingDateTime = DateTime.Now.Date,
                    BatchNumber = null,
                    BatchPath = null,
                    BatchCreatedDateTime = null,
                    TransportedDateTime = null,
                    CreatedDateTime = DateTime.Now.AddHours(-2),
                    Status = "READY",
                    ClientSession = new ClientSession
                    {
                        Id = 1,
                        SessionId = new Guid().ToString()
                    },
                    Account = new Account
                    {
                        Id = 1,
                        AccountNumber = "38475983",
                        BSBCode = "063945",
                        AccountType = "DDA",
                    },
                    Vouchers = new List<Voucher>
                    {
                        new Voucher
                        {
                            Id = 31,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000457",
                            SequenceId = 0,
                            VoucherType = "HDR",
                            TransactionCode = "22",
                            AuxDom = "",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "22",
                            AccountNumber = "0000000023",
                            AmountInCents = 0,
                            IsNonPostingCheque = false,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 31,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                RearImage =
                                    "/9j/4AAQSkZJRgABAQEAYABgAAD/4QDgRXhpZgAATU0AKgAAAAgADwD+AAQAAAABAAAAAAEAAAQAAAABAAAE8AEBAAQAAAABAAABsAECAAMAAAADAAAAwgEDAAMAAAABAAUAAAEGAAMAAAABAAIAAAERAAQAAAABAAAACAEVAAMAAAABAAMAAAEWAAQAAAABAAABsAEXAAQAAAABAACrCgEaAAUAAAABAAAAyAEbAAUAAAABAAAA0AEcAAMAAAABAAEAAAEoAAMAAAABAAIAAAE9AAMAAAABAAIAAAAAAAAACAAIAAgAAXcAAAAD6AABdwAAAAPo/9sAQwACAQECAQECAgICAgICAgMFAwMDAwMGBAQDBQcGBwcHBgcHCAkLCQgICggHBwoNCgoLDAwMDAcJDg8NDA4LDAwM/9sAQwECAgIDAwMGAwMGDAgHCAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgAIABlAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/LOiiv0S/wCCZ/8AwTs+Af7S/wCwbqfiv4reKtY8FeLvFnxFb4e+FtcWcf2fpt2NNivoPNiICsJWMyMXcA/u1UxsSx6G7AfnbXrHxQ/YX+MXwX+GOjeNPFHw38W6P4T160S+s9WlsHa1MT8oZHXIhLDBCybWIIIGDmv1A/Yt/wCCHXhX/gnb4k8QfGf9rPxH4PXwr4Eu2XQ7AS/abLVZBzHdSoy7pCf+WVsFLs4ywwoVvvD4lftYaP8Atwf8EWfi58UPD+n6hpeieJPAHjBLK3vSv2jyraLULQO4UkKX8jftBO3fjJxkw5dgP5d6KK7D4efDWx8TeFNa17VtS1Cw0nQ5IIZv7P00X9wGmLbGKNLEiR/IQXZx8zIADu4vpcDj6K9Ah+GHhm1+H8niC+8SawttNq1zplgtnoiTfaBDHFIJJN9zGY9wlUbQHxg8mptU+F3g+1+F0PiS38UeJZvtl3Np9vbSeHYI83EUcUh3uL1tsZ81RuAY8H5emTfT0/G1vzDe3nf8N/yPOaK9X+IP7NMOh3uoWXhvWr3xBqWj63DoN7bXGmrZHz5t4haJhNIHRmjdTu2EcHGDkcx4w8EeHdERrHSfEl5ruvQTJBJDDpWyynY8N9nm80vJhsAboY9wyR2BlST2/rZ/qg8/6/rRnH0V2mr/ALPvizRdT06zm063kuNUvxpUK2+oW1wEuyQBbymORhDJz9yUqeDxwcN1L4BeLNI1bTrK402JZ9UaZISt9bvHG0I3TLLIrlIWiXmRZSpQcsAOaoP6/r7mcbRW1ffDrWbXWLmxhtU1Kez2ea+myR6lEu5cr+8t2dD3HDHkEHkEUUtegGLX6J/8E1/28/2d/gN+wBqXhP4x6FrPjDxN4R+JLfELw54aitc2Wt3P9mRWMAmlIMYSNvNd1kwOIyFl5jP52UU5agftL+xl/wAFrfBP/BTvXfEHwT/ar8L+E4dE8cXxbw1eJGYbOxkb5YrR5C2+KYZ/d3IZSWYqcZGft74ifsmaX+w1/wAEUvi18LtE1O+1fR/DPgDxg1ndXiqtw0VzFqF2qPt+Usgn2FgAG2bsLnA/l8r2T4r/APBQn43fHD4XaT4L8VfE7xdq/hXRbRLG30x70xwSwqMKJwm37QQOA029gOM4qHHsB43XZfB/xRpfg7UZNQm8QeLPDOrQOv2a70W1jufMjIO+N1aaEgH5f4mBGQV71xtFaXE1c9Y8X/tJSP4O1DS/Ccms+E4dS1+91Ka0sLg21s9rNFCiRMI2UHBjfKbQgDYHHA5G68cWk3wc0vw8I7j7dY61c6jI5UeUY5IbdFAOc7gYmyMYwRyeccrRSjpr6fhaw+3lf8d/zPWPiP8AtTap4l+L665pvlw6Pp+uDWrKz+wwWbSurAq1wYQDK+0bdzs7AM2Dyc5Y8WeDfBnjXTPFHhtvElxfWOpwagml6hbQxwWwRxI0f2lJWabkbQfKjyDuPI2nzuipjHltbp/wPv2Qcqaa6PT+vvPYNA+MHg/4eazYSaJ/wkl9a3Hiey17UDf2sEctrFbM7LDFtlbzXPnPmRjHnavyjJxmeH/jBp9noel2X23WNHms/EF/qsl5bWMV4RDPBDGsflPKiybvLZXRyEKOQdwJWvMqKqy/r5f5Bu7vf+l+p758O/2yrH4J+MPEF14b8NW91Za3HZrIZQmn75YUcPKII98cPmM5by0O1e3BwCvA6KXsYS95/r+gvfex/9k=",
                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                            }
                        },
                        new Voucher
                        {
                            Id = 32,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000459",
                            SequenceId = 1,
                            VoucherType = "DBT",
                            TransactionCode = "09",
                            AuxDom = "400002003",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "063945",
                            AccountNumber = "38475983",
                            AmountInCents = 12300,
                            IsNonPostingCheque = false,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 32,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                RearImage =
                                    "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",
                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                            }
                        },
                        new Voucher
                        {
                            Id = 33,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000460",
                            SequenceId = 2,
                            VoucherType = "DBT",
                            TransactionCode = "09",
                            AuxDom = "400023837",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "063945",
                            AccountNumber = "38475983",
                            AmountInCents = 21200,
                            IsNonPostingCheque = false,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 33,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                RearImage =
                                    "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",
                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                            }
                        },
                        new Voucher
                        {
                            Id = 34,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000461",
                            SequenceId = 3,
                            VoucherType = "DBT",
                            TransactionCode = "09",
                            AuxDom = "400023838",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "063945",
                            AccountNumber = "38475983",
                            AmountInCents = 100,
                            IsNonPostingCheque = false,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 34,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                RearImage =
                                    "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",
                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                            }
                        },
                        new Voucher
                        {
                            Id = 35,
                            PaymentInstructionId = id,
                            TrackingId = "",
                            SequenceId = 1,
                            VoucherType = "DBT",
                            TransactionCode = "09",
                            AuxDom = "400002007",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "063945",
                            AccountNumber = "38475983",
                            AmountInCents = 123000,
                            IsNonPostingCheque = true,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 35,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "",
                                RearImage =
                                    "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",
                                RearImageSHA = ""
                            }
                        },
                        new Voucher
                        {
                            Id = 36,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000460",
                            SequenceId = 2,
                            VoucherType = "DBT",
                            TransactionCode = "09",
                            AuxDom = "400002008",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "063945",
                            AccountNumber = "38475983",
                            AmountInCents = 321044,
                            IsNonPostingCheque = true,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 36,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "",
                                RearImage =
                                    "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",
                                RearImageSHA = ""
                            }
                        },
                        new Voucher
                        {
                            Id = 37,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000461",
                            SequenceId = 3,
                            VoucherType = "DBT",
                            TransactionCode = "09",
                            AuxDom = "400002009",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "063945",
                            AccountNumber = "38475983",
                            AmountInCents = 12234,
                            IsNonPostingCheque = true,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 37,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "",
                                RearImage =
                                    "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",
                                RearImageSHA = ""
                            }
                        },
                        new Voucher
                        {
                            Id = 38,
                            PaymentInstructionId = id,
                            TrackingId = "NAB20151102101000458",
                            SequenceId = 3,
                            VoucherType = "CRT",
                            TransactionCode = "94",
                            AuxDom = "400002009",
                            ProcessingDateTime = DateTime.Now.Date,
                            BSB = "92",
                            AccountNumber = "38475983",
                            AmountInCents = 33600,
                            IsNonPostingCheque = false,
                            VoucherImage = new VoucherImage
                            {
                                VoucherId = 38,
                                FrontImage =
                                    "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",
                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                RearImage =
                                    "/9j/4AAQSkZJRgABAQEAYABgAAD/4QDgRXhpZgAATU0AKgAAAAgADwD+AAQAAAABAAAAAAEAAAQAAAABAAAE8AEBAAQAAAABAAABsAECAAMAAAADAAAAwgEDAAMAAAABAAUAAAEGAAMAAAABAAIAAAERAAQAAAABAAAACAEVAAMAAAABAAMAAAEWAAQAAAABAAABsAEXAAQAAAABAACrCgEaAAUAAAABAAAAyAEbAAUAAAABAAAA0AEcAAMAAAABAAEAAAEoAAMAAAABAAIAAAE9AAMAAAABAAIAAAAAAAAACAAIAAgAAXcAAAAD6AABdwAAAAPo/9sAQwACAQECAQECAgICAgICAgMFAwMDAwMGBAQDBQcGBwcHBgcHCAkLCQgICggHBwoNCgoLDAwMDAcJDg8NDA4LDAwM/9sAQwECAgIDAwMGAwMGDAgHCAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgAIABlAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/LOiiv0S/wCCZ/8AwTs+Af7S/wCwbqfiv4reKtY8FeLvFnxFb4e+FtcWcf2fpt2NNivoPNiICsJWMyMXcA/u1UxsSx6G7AfnbXrHxQ/YX+MXwX+GOjeNPFHw38W6P4T160S+s9WlsHa1MT8oZHXIhLDBCybWIIIGDmv1A/Yt/wCCHXhX/gnb4k8QfGf9rPxH4PXwr4Eu2XQ7AS/abLVZBzHdSoy7pCf+WVsFLs4ywwoVvvD4lftYaP8Atwf8EWfi58UPD+n6hpeieJPAHjBLK3vSv2jyraLULQO4UkKX8jftBO3fjJxkw5dgP5d6KK7D4efDWx8TeFNa17VtS1Cw0nQ5IIZv7P00X9wGmLbGKNLEiR/IQXZx8zIADu4vpcDj6K9Ah+GHhm1+H8niC+8SawttNq1zplgtnoiTfaBDHFIJJN9zGY9wlUbQHxg8mptU+F3g+1+F0PiS38UeJZvtl3Np9vbSeHYI83EUcUh3uL1tsZ81RuAY8H5emTfT0/G1vzDe3nf8N/yPOaK9X+IP7NMOh3uoWXhvWr3xBqWj63DoN7bXGmrZHz5t4haJhNIHRmjdTu2EcHGDkcx4w8EeHdERrHSfEl5ruvQTJBJDDpWyynY8N9nm80vJhsAboY9wyR2BlST2/rZ/qg8/6/rRnH0V2mr/ALPvizRdT06zm063kuNUvxpUK2+oW1wEuyQBbymORhDJz9yUqeDxwcN1L4BeLNI1bTrK402JZ9UaZISt9bvHG0I3TLLIrlIWiXmRZSpQcsAOaoP6/r7mcbRW1ffDrWbXWLmxhtU1Kez2ea+myR6lEu5cr+8t2dD3HDHkEHkEUUtegGLX6J/8E1/28/2d/gN+wBqXhP4x6FrPjDxN4R+JLfELw54aitc2Wt3P9mRWMAmlIMYSNvNd1kwOIyFl5jP52UU5agftL+xl/wAFrfBP/BTvXfEHwT/ar8L+E4dE8cXxbw1eJGYbOxkb5YrR5C2+KYZ/d3IZSWYqcZGft74ifsmaX+w1/wAEUvi18LtE1O+1fR/DPgDxg1ndXiqtw0VzFqF2qPt+Usgn2FgAG2bsLnA/l8r2T4r/APBQn43fHD4XaT4L8VfE7xdq/hXRbRLG30x70xwSwqMKJwm37QQOA029gOM4qHHsB43XZfB/xRpfg7UZNQm8QeLPDOrQOv2a70W1jufMjIO+N1aaEgH5f4mBGQV71xtFaXE1c9Y8X/tJSP4O1DS/Ccms+E4dS1+91Ka0sLg21s9rNFCiRMI2UHBjfKbQgDYHHA5G68cWk3wc0vw8I7j7dY61c6jI5UeUY5IbdFAOc7gYmyMYwRyeccrRSjpr6fhaw+3lf8d/zPWPiP8AtTap4l+L665pvlw6Pp+uDWrKz+wwWbSurAq1wYQDK+0bdzs7AM2Dyc5Y8WeDfBnjXTPFHhtvElxfWOpwagml6hbQxwWwRxI0f2lJWabkbQfKjyDuPI2nzuipjHltbp/wPv2Qcqaa6PT+vvPYNA+MHg/4eazYSaJ/wkl9a3Hiey17UDf2sEctrFbM7LDFtlbzXPnPmRjHnavyjJxmeH/jBp9noel2X23WNHms/EF/qsl5bWMV4RDPBDGsflPKiybvLZXRyEKOQdwJWvMqKqy/r5f5Bu7vf+l+p758O/2yrH4J+MPEF14b8NW91Za3HZrIZQmn75YUcPKII98cPmM5by0O1e3BwCvA6KXsYS95/r+gvfex/9k=",
                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                            }
                        }
                    }
                };
            });
                
            dataAccess.Setup(x => x.GetPaymentInstructions()).Returns(() =>
                {
                    var paymentInstructions = new List<PaymentInstruction>();

                    if (paymentInstructionsToBeGenerated > 0)
                    {
                        for (var i = 1; i <= paymentInstructionsToBeGenerated; i++)
                        {
                            paymentInstructions.Add
                            (
                                new PaymentInstruction
                                {
                                    Id = 11,
                                    TotalTransactionAmountInCents = 33600,
                                    ChannelType = "AFS-MIB",
                                    ChequeCount = 3,
                                    TransactionNarrative = "XXXXX XXX XXX",
                                    TrackingId = "NAB20151102101000458",
                                    ClientSessionId = 1,
                                    AccountId = 1,
                                    ProcessingDateTime = DateTime.Now.Date,
                                    BatchNumber = null,
                                    BatchPath = null,
                                    BatchCreatedDateTime = null,
                                    TransportedDateTime = null,
                                    CreatedDateTime = DateTime.Now.AddHours(-2),
                                    Status = "READY",
                                    ClientSession = new ClientSession
                                    {
                                        Id = 1,
                                        SessionId = new Guid().ToString()
                                    },
                                    Account = new Account
                                    {
                                        Id = 1,
                                        AccountNumber = "38475983",
                                        BSBCode = "063945",
                                        AccountType = "DDA",
                                    },
                                    Vouchers = new List<Voucher>
                                    {
                                        new Voucher
                                        {
                                            Id = 31,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000457",
                                            SequenceId = 0,
                                            VoucherType = "HDR",
                                            TransactionCode = "22",
                                            AuxDom = "",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "22",
                                            AccountNumber = "0000000023",
                                            AmountInCents = 0,
                                            IsNonPostingCheque = false,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 31,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 32,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000459",
                                            SequenceId = 1,
                                            VoucherType = "DBT",
                                            TransactionCode = "09",
                                            AuxDom = "400002003",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "063945",
                                            AccountNumber = "38475983",
                                            AmountInCents = 12300,
                                            IsNonPostingCheque = false,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 32,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 33,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000460",
                                            SequenceId = 2,
                                            VoucherType = "DBT",
                                            TransactionCode = "09",
                                            AuxDom = "400023837",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "063945",
                                            AccountNumber = "38475983",
                                            AmountInCents = 21200,
                                            IsNonPostingCheque = false,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 33,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 34,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000461",
                                            SequenceId = 3,
                                            VoucherType = "DBT",
                                            TransactionCode = "09",
                                            AuxDom = "400023838",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "063945",
                                            AccountNumber = "38475983",
                                            AmountInCents = 100,
                                            IsNonPostingCheque = false,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 34,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 35,
                                            PaymentInstructionId = 11,
                                            TrackingId = "",
                                            SequenceId = 1,
                                            VoucherType = "DBT",
                                            TransactionCode = "09",
                                            AuxDom = "400002007",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "063945",
                                            AccountNumber = "38475983",
                                            AmountInCents = 123000,
                                            IsNonPostingCheque = true,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 35,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = ""
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 36,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000460",
                                            SequenceId = 2,
                                            VoucherType = "DBT",
                                            TransactionCode = "09",
                                            AuxDom = "400002008",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "063945",
                                            AccountNumber = "38475983",
                                            AmountInCents = 321044,
                                            IsNonPostingCheque = true,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 36,
                                                FrontImage =
                                                    "/9j/4AAQSkZJRgABAQEAYABgAAD/4QDgRXhpZgAATU0AKgAAAAgADwD+AAQAAAABAAAAAAEAAAQAAAABAAAE8AEBAAQAAAABAAABsAECAAMAAAADAAAAwgEDAAMAAAABAAUAAAEGAAMAAAABAAIAAAERAAQAAAABAAAACAEVAAMAAAABAAMAAAEWAAQAAAABAAABsAEXAAQAAAABAACrCgEaAAUAAAABAAAAyAEbAAUAAAABAAAA0AEcAAMAAAABAAEAAAEoAAMAAAABAAIAAAE9AAMAAAABAAIAAAAAAAAACAAIAAgAAXcAAAAD6AABdwAAAAPo/9sAQwACAQECAQECAgICAgICAgMFAwMDAwMGBAQDBQcGBwcHBgcHCAkLCQgICggHBwoNCgoLDAwMDAcJDg8NDA4LDAwM/9sAQwECAgIDAwMGAwMGDAgHCAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgAIABlAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/LOiiv0S/wCCZ/8AwTs+Af7S/wCwbqfiv4reKtY8FeLvFnxFb4e+FtcWcf2fpt2NNivoPNiICsJWMyMXcA/u1UxsSx6G7AfnbXrHxQ/YX+MXwX+GOjeNPFHw38W6P4T160S+s9WlsHa1MT8oZHXIhLDBCybWIIIGDmv1A/Yt/wCCHXhX/gnb4k8QfGf9rPxH4PXwr4Eu2XQ7AS/abLVZBzHdSoy7pCf+WVsFLs4ywwoVvvD4lftYaP8Atwf8EWfi58UPD+n6hpeieJPAHjBLK3vSv2jyraLULQO4UkKX8jftBO3fjJxkw5dgP5d6KK7D4efDWx8TeFNa17VtS1Cw0nQ5IIZv7P00X9wGmLbGKNLEiR/IQXZx8zIADu4vpcDj6K9Ah+GHhm1+H8niC+8SawttNq1zplgtnoiTfaBDHFIJJN9zGY9wlUbQHxg8mptU+F3g+1+F0PiS38UeJZvtl3Np9vbSeHYI83EUcUh3uL1tsZ81RuAY8H5emTfT0/G1vzDe3nf8N/yPOaK9X+IP7NMOh3uoWXhvWr3xBqWj63DoN7bXGmrZHz5t4haJhNIHRmjdTu2EcHGDkcx4w8EeHdERrHSfEl5ruvQTJBJDDpWyynY8N9nm80vJhsAboY9wyR2BlST2/rZ/qg8/6/rRnH0V2mr/ALPvizRdT06zm063kuNUvxpUK2+oW1wEuyQBbymORhDJz9yUqeDxwcN1L4BeLNI1bTrK402JZ9UaZISt9bvHG0I3TLLIrlIWiXmRZSpQcsAOaoP6/r7mcbRW1ffDrWbXWLmxhtU1Kez2ea+myR6lEu5cr+8t2dD3HDHkEHkEUUtegGLX6J/8E1/28/2d/gN+wBqXhP4x6FrPjDxN4R+JLfELw54aitc2Wt3P9mRWMAmlIMYSNvNd1kwOIyFl5jP52UU5agftL+xl/wAFrfBP/BTvXfEHwT/ar8L+E4dE8cXxbw1eJGYbOxkb5YrR5C2+KYZ/d3IZSWYqcZGft74ifsmaX+w1/wAEUvi18LtE1O+1fR/DPgDxg1ndXiqtw0VzFqF2qPt+Usgn2FgAG2bsLnA/l8r2T4r/APBQn43fHD4XaT4L8VfE7xdq/hXRbRLG30x70xwSwqMKJwm37QQOA029gOM4qHHsB43XZfB/xRpfg7UZNQm8QeLPDOrQOv2a70W1jufMjIO+N1aaEgH5f4mBGQV71xtFaXE1c9Y8X/tJSP4O1DS/Ccms+E4dS1+91Ka0sLg21s9rNFCiRMI2UHBjfKbQgDYHHA5G68cWk3wc0vw8I7j7dY61c6jI5UeUY5IbdFAOc7gYmyMYwRyeccrRSjpr6fhaw+3lf8d/zPWPiP8AtTap4l+L665pvlw6Pp+uDWrKz+wwWbSurAq1wYQDK+0bdzs7AM2Dyc5Y8WeDfBnjXTPFHhtvElxfWOpwagml6hbQxwWwRxI0f2lJWabkbQfKjyDuPI2nzuipjHltbp/wPv2Qcqaa6PT+vvPYNA+MHg/4eazYSaJ/wkl9a3Hiey17UDf2sEctrFbM7LDFtlbzXPnPmRjHnavyjJxmeH/jBp9noel2X23WNHms/EF/qsl5bWMV4RDPBDGsflPKiybvLZXRyEKOQdwJWvMqKqy/r5f5Bu7vf+l+p758O/2yrH4J+MPEF14b8NW91Za3HZrIZQmn75YUcPKII98cPmM5by0O1e3BwCvA6KXsYS95/r+gvfex/9k=",
                                                FrontImageSHA = "",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = ""
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 37,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000461",
                                            SequenceId = 3,
                                            VoucherType = "DBT",
                                            TransactionCode = "09",
                                            AuxDom = "400002009",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "063945",
                                            AccountNumber = "38475983",
                                            AmountInCents = 12234,
                                            IsNonPostingCheque = true,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 37,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = ""
                                            }
                                        },
                                        new Voucher
                                        {
                                            Id = 38,
                                            PaymentInstructionId = 11,
                                            TrackingId = "NAB20151102101000458",
                                            SequenceId = 3,
                                            VoucherType = "CRT",
                                            TransactionCode = "94",
                                            AuxDom = "400002009",
                                            ProcessingDateTime = DateTime.Now.Date,
                                            BSB = "92",
                                            AccountNumber = "38475983",
                                            AmountInCents = 33600,
                                            IsNonPostingCheque = false,
                                            VoucherImage = new VoucherImage
                                            {
                                                VoucherId = 38,
                                                FrontImage =
                                                    "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",
                                                FrontImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6",
                                                RearImage =
                                                    "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",
                                                RearImageSHA = "92DA6CF4A46B342DEC90724BADBA40623E83A8D6"
                                            }
                                        }
                                    }
                                }
                            );
                    }
                }

                return paymentInstructions;
            });
            
            return dataAccess.Object;
        }

        private static IDipsPayloadMetadataCreator GetDipsPayloadMetadataCreatorMock(bool good = true)
        {
            var creator = new Mock<IDipsPayloadMetadataCreator>();
            creator.Setup(x => x.GetScannedBatchMetadata(It.IsAny<PaymentInstruction>(), It.IsAny<DirectoryInfo>()));
            return creator.Object;
        }

        private static IDipsPayloadServiceConfiguration GetDipsPayloadServiceConfigurationMock(bool good = true)
        {
            var configuration = new Mock<IDipsPayloadServiceConfiguration>();

            configuration.SetupGet(x => x.PayloadFileSystemLocation)
                .Returns(new PayloadFileSystemLocationElement {Path = @"\\NRYE745-B31RY52\DipsPayloads\Payloads"});

            configuration.SetupGet(x => x.PayloadAccountNumber)
                .Returns(new PayloadAccountNumberElement {Header = "0000000023"});

            configuration.SetupGet(x => x.PayloadBsbNumber)
                .Returns(new PayloadBsbNumberElement {Header = "22", Credit = "94"});

            configuration.SetupGet(x => x.PayloadTransactionCode)
                .Returns(new PayloadTransactionCodeElement {Header = "22", Debit = "09", Credit = "94"});

            configuration.SetupGet(x => x.PayloadVoucherType)
                .Returns(new PayloadVoucherTypeElement {Header = "HDR", Debit = "DBT", Credit = "CRT"});

            configuration.SetupGet(x => x.PayloadProcessingDetails)
                .Returns(new PayloadProcessingDetailsElement
                    {
                        Operator = "DipsPayloadService",
                        BatchClient = "NabChq",
                        BatchType = "OTC_vouchers",
                        BatchAccountNumber = "0000000023",
                        WorkType = "NABCHQ_POD",
                        UnitId = "099",
                        State = "VIC",
                        CollectingBank = "083340",
                        CaptureBsb = "083340",
                        Source = "DPSE",
                        DocumentReferenceNumberPreFix = "299",
                        HeaderVoucherFrontImagePath = @"\\NRYE745-B31RY52\DipsPayloads\Source\Header_Voucher_Front.jpg",
                        HeaderVoucherRearImagePath = @"\\NRYE745-B31RY52\DipsPayloads\Source\Header_Voucher_Rear.jpg",
                        CreditVoucherFrontImagePath = @"\\NRYE745-B31RY52\DipsPayloads\Source\Credit_Voucher_Front.jpg",
                        CreditVoucherRearImagePath = @"\\NRYE745-B31RY52\DipsPayloads\Source\Credit_Voucher_Rear.jpg"
                    }
                );

            return configuration.Object;
        }

        [Fact]
        public void TestGeneratePayloadWithPaymentInstructionAvailableToProcess()
        {
            var dipsPayloadServiceConfiguration = GetDipsPayloadServiceConfigurationMock();
            var dipsPayloadDataAccess = GetPaymentInstructionDataAccessMock(1);
            var dipsPayloadImageCreator = GetDipsPayloadImagesCreatorMock();
            var dipsPayloadMetadataCreator = GetDipsPayloadMetadataCreatorMock();
            var fileSystem = GetFileSystemMock();
            var loggingProxy = GetLoggingProxyMock();
            var auditProxy = GetAuditProxyMock();

            var batchCreator = new PayloadBatchCreator(
                dipsPayloadDataAccess,
                loggingProxy, 
                auditProxy, 
                dipsPayloadServiceConfiguration, 
                dipsPayloadMetadataCreator, 
                dipsPayloadImageCreator,
                fileSystem
            );

            var result = batchCreator.GeneratePayload();

            Assert.Equal(result.HasException, false);
            Assert.Equal(result.HasWarning, false);
            Assert.Equal(result.ProcessedBatchCount, 1);
        }

        [Fact]
        public void TestGeneratePayloadWithMultiplePaymentInstructionsAvailableToProcess()
        {
            var dipsPayloadServiceConfiguration = GetDipsPayloadServiceConfigurationMock();
            var dipsPayloadDataAccess = GetPaymentInstructionDataAccessMock(5);
            var dipsPayloadImageCreator = GetDipsPayloadImagesCreatorMock();
            var dipsPayloadMetadataCreator = GetDipsPayloadMetadataCreatorMock();
            var fileSystem = GetFileSystemMock();
            var loggingProxy = GetLoggingProxyMock();
            var auditProxy = GetAuditProxyMock();

            var batchCreator = new PayloadBatchCreator(
                dipsPayloadDataAccess,
                loggingProxy,
                auditProxy,
                dipsPayloadServiceConfiguration,
                dipsPayloadMetadataCreator,
                dipsPayloadImageCreator,
                fileSystem
            );

            var result = batchCreator.GeneratePayload();

            Assert.Equal(result.HasException, false);
            Assert.Equal(result.HasWarning, false);
            Assert.Equal(result.ProcessedBatchCount, 5);
        }

        [Fact]
        public void TestGeneratePayloadWithNoPaymentInstructionsAvailableToProcess()
        {
            var dipsPayloadServiceConfiguration = GetDipsPayloadServiceConfigurationMock();
            var dipsPayloadDataAccess = GetPaymentInstructionDataAccessMock(0);
            var dipsPayloadImageCreator = GetDipsPayloadImagesCreatorMock();
            var dipsPayloadMetadataCreator = GetDipsPayloadMetadataCreatorMock();
            var fileSystem = GetFileSystemMock();
            var loggingProxy = GetLoggingProxyMock();
            var auditProxy = GetAuditProxyMock();

            var batchCreator = new PayloadBatchCreator(
                dipsPayloadDataAccess,
                loggingProxy,
                auditProxy,
                dipsPayloadServiceConfiguration,
                dipsPayloadMetadataCreator,
                dipsPayloadImageCreator,
                fileSystem
            );

            var result = batchCreator.GeneratePayload();

            Assert.Equal(result.HasException, false);
            Assert.Equal(result.HasWarning, false);
            Assert.Equal(result.ProcessedBatchCount, 0);
        }

        [Fact]
        public void TestGeneratePayloadWithParticularPaymentInstructionIdAvailable()
        {
            var dipsPayloadServiceConfiguration = GetDipsPayloadServiceConfigurationMock();
            var dipsPayloadDataAccess = GetPaymentInstructionDataAccessMock(0, true);
            var dipsPayloadImageCreator = GetDipsPayloadImagesCreatorMock();
            var dipsPayloadMetadataCreator = GetDipsPayloadMetadataCreatorMock();
            var fileSystem = GetFileSystemMock();
            var loggingProxy = GetLoggingProxyMock();
            var auditProxy = GetAuditProxyMock();

            var batchCreator = new PayloadBatchCreator(
                dipsPayloadDataAccess,
                loggingProxy,
                auditProxy,
                dipsPayloadServiceConfiguration,
                dipsPayloadMetadataCreator,
                dipsPayloadImageCreator,
                fileSystem
            );

            var result = batchCreator.GeneratePayload(101);

            Assert.Equal(result.HasException, false);
            Assert.Equal(result.HasWarning, false);
            Assert.Equal(result.PaymentInstructionId, 101);
        }

        [Fact]
        public void TestGeneratePayloadWithParticularPaymentInstructionIdNotAvailable()
        {
            var dipsPayloadServiceConfiguration = GetDipsPayloadServiceConfigurationMock();
            var dipsPayloadDataAccess = GetPaymentInstructionDataAccessMock(0, false);
            var dipsPayloadImageCreator = GetDipsPayloadImagesCreatorMock();
            var dipsPayloadMetadataCreator = GetDipsPayloadMetadataCreatorMock();
            var fileSystem = GetFileSystemMock();
            var loggingProxy = GetLoggingProxyMock();
            var auditProxy = GetAuditProxyMock();

            var batchCreator = new PayloadBatchCreator(
                dipsPayloadDataAccess,
                loggingProxy,
                auditProxy,
                dipsPayloadServiceConfiguration,
                dipsPayloadMetadataCreator,
                dipsPayloadImageCreator,
                fileSystem
            );

            var result = batchCreator.GeneratePayload(1001);

            Assert.Equal(result.HasException, true);
            Assert.Equal(result.BusinessException.ErrorCode, "DPSE-6003");
            Assert.Equal(result.BusinessException.Message, "There is no payment instruction found with id 1001 to be processed.");
            Assert.Equal(result.BusinessException.ExceptionType, DpseBusinessExceptionType.BusinessRule);
            Assert.Equal(result.HasWarning, false);
            Assert.Equal(result.PaymentInstructionId, 1001);
        }
    }
}