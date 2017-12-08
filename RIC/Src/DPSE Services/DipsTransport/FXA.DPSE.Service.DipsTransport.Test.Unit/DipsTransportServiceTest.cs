using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Business;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Business.PayloadTransport;
using FXA.DPSE.Service.DipsTransport.Business.SimpleTransport;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Common.Converters;
using FXA.DPSE.Service.DipsTransport.DataAccess;
using FXA.DPSE.Service.DipsTransport.Test.Unit.Extensions;
using FXA.DPSE.Service.DTO.DipsTransport;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.DipsTransport.Test.Unit
{
    public class DipsTransportServiceTest
    {
        private IDipsTransportServiceConfiguration Configuration { get; set; }

        public DipsTransportServiceTest()
        {
            var config = new Mock<IDipsTransportServiceConfiguration>();

            config.SetupGet(prop => prop.Transports).Returns(new List<TransportElement>
                {
                    new TransportElement
                    {
                        Name = "dipsPayload",
                        Source = new SourceElement
                        {
                            Type = SourceTypes.Path,
                            Path = @"\\NRYE745-B31RY52\DipsPayloads\Payloads",
                            RegEx = @".jpg|.tiff|.xml"
                        },
                        Destination = new DestinationElement
                        {
                            Type = DestinationTypes.Sftp,
                            Path = @"/DipsPayloads",
                            Connection = new ConnectionElement
                            {
                                Server="127.0.0.1",
                                Port=22,
                                User="gmpatel",
                                Password="",
                                CertificatePath=@"C:\Temp\SFTP\Certificate\openssh-key-20151007.ppk",
                                UseCertificateInsteadPasswordForAuthorization=true,
                                ConnectionTimeOutMiliSeconds=15000,
                                IdleTimeoutMiliSeconds=15000
                            }
                        },
                        TempLocation = new TempLocationElement
                        {
                            Type = TempLocationTypes.Path,
                            Path = @"\\NRYE745-B31RY52\DipsPayloadsTempLocation"
                        }
                    },
                    new TransportElement
                    {
                        Name = "dipsEod",
                        Source = new SourceElement
                        {
                            Type = SourceTypes.Path,
                            Path = @"\\NRYE745-B31RY52\DipsPayloads\Payloads",
                            RegEx = @".xml"
                        },
                        Destination = new DestinationElement
                        {
                            Type = DestinationTypes.Sftp,
                            Path = @"/DipsPayloads",
                            Connection = new ConnectionElement
                            {
                                Server="127.0.0.1",
                                Port=22,
                                User="gmpatel",
                                Password="",
                                CertificatePath=@"C:\Temp\SFTP\Certificate\openssh-key-20151007.ppk",
                                UseCertificateInsteadPasswordForAuthorization=true,
                                ConnectionTimeOutMiliSeconds=15000,
                                IdleTimeoutMiliSeconds=15000
                            }
                        },
                        TempLocation = new TempLocationElement
                        {
                            Type = TempLocationTypes.Path,
                            Path = @"\\NRYE745-B31RY52\DipsPayloadsTempLocation"
                        }
                    }
                }
            );

            Configuration = config.Object;
        }

        private static IFileSystem GetFileSystemMock(bool good = true)
        {
            var obj = new Mock<IFileSystem>();

            if (good)
            {
                obj.Setup(x => x.FileExists(It.IsAny<FileInfo>())).Returns(true);
                obj.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
                obj.Setup(x => x.GetSHA1String(It.IsAny<FileInfo>())).Returns(new string('X', 56));
                obj.Setup(x => x.GetFilesFromDirectory(It.IsAny<DirectoryInfo>()))
                    .Returns(
                        (DirectoryInfo directory) => new FileInfo[]
                            {
                                new FileInfo(string.Format(@"{0}\OUTCLEARINGSPKG_XXXX2015_0000000XXX.XML", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000492_FRONT.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000492_FRONT.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000492_REAR.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000492_REAR.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000493_FRONT.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000493_FRONT.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000493_REAR.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000493_REAR.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000494_FRONT.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000494_FRONT.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000494_REAR.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000494_REAR.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000495_FRONT.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000495_FRONT.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000495_REAR.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000495_REAR.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000496_FRONT.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000496_FRONT.TIF", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000496_REAR.JPG", directory.FullName)),
                                new FileInfo(string.Format(@"{0}\VOUCHER_XXXX2015_299000496_REAR.TIF", directory.FullName)),
                            }
                    );
            }
            else
            {
                obj.Setup(x => x.FileExists(It.IsAny<FileInfo>())).Returns(false);
                obj.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
                obj.Setup(x => x.GetSHA1String(It.IsAny<FileInfo>())).Returns(default(string));
                obj.Setup(x => x.GetFilesFromDirectory(It.IsAny<DirectoryInfo>()))
                    .Returns((DirectoryInfo directory) => null);
            }

            return obj.Object;
        }

        private static IDipsTransportZipCreator GetDipsTransportZipCreatorMock(bool good = true)
        {
            var obj = new Mock<IDipsTransportZipCreator>();

            if (good)
            {
                obj.Setup(x => x.CreateZip(It.IsAny<FileInfo>(), It.IsAny<IList<FileInfo>>(), It.IsAny<bool>()))
                    .Returns((FileInfo target, IList<FileInfo> files, bool removeOriginalDirectoryPaths) 
                            => target
                    );

                obj.Setup(x => x.CreateZip(It.IsAny<FileInfo>(), It.IsAny<FileInfo>(), It.IsAny<bool>()))
                    .Returns((FileInfo target, FileInfo file, bool removeOriginalDirectoryPaths)
                            => target
                    );
            }
            else
            {
                obj.Setup(x => x.CreateZip(It.IsAny<FileInfo>(), It.IsAny<IList<FileInfo>>(), It.IsAny<bool>()))
                    .Returns(default(FileInfo));

                obj.Setup(x => x.CreateZip(It.IsAny<FileInfo>(), It.IsAny<FileInfo>(), It.IsAny<bool>()))
                    .Returns(default(FileInfo));
            }
                
            return obj.Object;
        }

        private static IDipsTransportPgpCreator GetDipsTransportPgpCreatorMock(bool good = true)
        {
            var obj = new Mock<IDipsTransportPgpCreator>();

            if (good)
            {
                obj.Setup(x => x.CreatePgp(It.IsAny<FileInfo>(), It.IsAny<bool>()))
                    .Returns((FileInfo file, bool removeOriginalFile)
                            => new FileInfo(string.Format(@"{0}.PGP", file.FullName))
                    );
            }
            else
            {
                obj.Setup(x => x.CreatePgp(It.IsAny<FileInfo>(), It.IsAny<bool>()))
                    .Returns(default(FileInfo));
            }

            return obj.Object;
        }

        private static IDipsTransportDataAccess GetDipsTransportDataAccessMock(bool data = false)
        {
            var obj = new Mock<IDipsTransportDataAccess>();

            if (data)
            {
                obj.Setup(x => x.GetPaymentInstruction(It.IsAny<long>())).Returns
                    (
                        (long id) => new PaymentInstruction
                            {
                                Id = id,
                                BatchPath = string.Format(@"\\NRYE745-B31RY52\DipsPayloads\Payloads\OUTCLEARINGSPKG_02112015_{0}", id.ToString("0000000000"))
                            }
                    );

                obj.Setup(x => x.GetTransportTransmissionsToBeProcessed()).Returns
                (
                    new List<Transmission>
                    {
                        new Transmission
                        {
                            Id = 4,
                            CreationDateTime = DateTime.Today.AddDays(-1),
                            FilePath = @"\\NRYE745-B31RY52\DipsPayloadsTempLocation\DPSE-FXA-PAYLOAD-000000004.ZIP.PGP",
                            FileName = @"DPSE-FXA-PAYLOAD-000000004.ZIP.PGP",
                            FileSHAHash = @"91BCFADED4EE2BBF5BF22376475391EAD817290A",
                            TransactionCount = 6,
                            RetryCount = 2,
                            Status = "RETRY",
                            TransmissionBatches = new List<TransmissionBatch>
                            {
                                new TransmissionBatch
                                {
                                    TransmissionId = 4,
                                    PaymentInstructionId = 8,
                                    BatchNumber = "00000008",
                                    FilePath = @"\\NRYE745-B31RY52\DipsPayloadsTempLocation\OUTCLEARINGSPKG_02112015_0000000008.ZIP",
                                    FileName = @"OUTCLEARINGSPKG_02112015_0000000008.ZIP",
                                    TransactionCount = 3
                                },
                                new TransmissionBatch
                                {
                                    TransmissionId = 4,
                                    PaymentInstructionId = 9,
                                    BatchNumber = "00000009",
                                    FilePath = @"\\NRYE745-B31RY52\DipsPayloadsTempLocation\OUTCLEARINGSPKG_02112015_0000000009.ZIP",
                                    FileName = @"OUTCLEARINGSPKG_02112015_0000000009.ZIP",
                                    TransactionCount = 3
                                }
                            }
                        },
                        new Transmission
                        {
                            Id = 5,
                            CreationDateTime = DateTime.Today.AddDays(-1),
                            FilePath = null,
                            FileName = null,
                            FileSHAHash = null,
                            TransactionCount = 6,
                            RetryCount = 0,
                            Status = "READY",
                            TransmissionBatches = new List<TransmissionBatch>
                            {
                                new TransmissionBatch
                                {
                                    TransmissionId = 5,
                                    PaymentInstructionId = 10,
                                    BatchNumber = "00000010",
                                    FilePath = null, // @"\\NRYE745-B31RY52\DipsPayloadsTempLocation\OUTCLEARINGSPKG_02112015_0000000010.ZIP",
                                    FileName = null, // @"OUTCLEARINGSPKG_02112015_0000000008.ZIP",
                                    TransactionCount = 3
                                },
                                new TransmissionBatch
                                {
                                    TransmissionId = 6,
                                    PaymentInstructionId = 11,
                                    BatchNumber = "00000011",
                                    FilePath = null, // @"\\NRYE745-B31RY52\DipsPayloadsTempLocation\OUTCLEARINGSPKG_02112015_0000000011.ZIP",
                                    FileName = null, // @"OUTCLEARINGSPKG_02112015_0000000009.ZIP",
                                    TransactionCount = 3
                                }
                            }
                        }
                    }
                );
            }
            
            return obj.Object;
        }

        private static IDipsTransportMetadataCreator GetDipsTransportMetadataCreator(bool good = true)
        {
            var obj = new Mock<IDipsTransportMetadataCreator>();

            if (good)
                obj.Setup(x => x.GenerateMetadata(It.IsAny<DateTime>(), It.IsAny<IList<Transmission>>(), It.IsAny<DirectoryInfo>()))
                    .Returns(new FileInfo(string.Format(@"C:\Temp\{0}.XML", Guid.NewGuid().ToString().ToUpper())));
            else
                obj.Setup(x => x.GenerateMetadata(It.IsAny<DateTime>(), It.IsAny<IList<Transmission>>(), It.IsAny<DirectoryInfo>()))
                    .Returns(default(FileInfo));

            return obj.Object;
        }

        private static DipsTransportPayloadRequest GetDipsTransportPayloadRequest()
        {
            return new DipsTransportPayloadRequest
            {
                RequestGuid = Guid.NewGuid().ToString(),
                IpAddressV4 = "127.0.0.1",
                RequestUtc = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T"),
                MessageVersion = "v2.0"
            };
        }

        private static DipsTransportBusinessData GetDipsTransportBusinessData()
        {
            return new DipsTransportBusinessData
            {
                RequestGuid = Guid.NewGuid().ToString(),
                IpAddressV4 = "127.0.0.1",
                RequestUtc = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T"),
                MessageVersion = "v2.0"
            };
        }

        [Fact]
        public void PayloadTransportTest2WithNoDataToBeTransported()
        {
            var businessData = GetDipsTransportBusinessData();

            var payloadConfig = Configuration.Transports.First();

            var dipsTransportDataAccess = GetDipsTransportDataAccessMock();
            var dipsTransportZipCreator = GetDipsTransportZipCreatorMock();
            var dipsTransportPgpCreator = GetDipsTransportPgpCreatorMock();
            var fileSystem = GetFileSystemMock();

            var pathToSftpTransporter = new Mock<PathToSftpTransportProcessor>(payloadConfig);
            
            pathToSftpTransporter.Setup(x => x.Transport(It.IsAny<DipsTransportBusinessData>()))
                .Returns(new DipsTransportBusinessResult());

            var payloadTransportProcessor = new DefaultPayloadTransportProcessor(
                pathToSftpTransporter.Object, 
                dipsTransportDataAccess, 
                dipsTransportZipCreator, 
                dipsTransportPgpCreator,
                fileSystem
            );

            var response = payloadTransportProcessor.Transport(businessData);
            
            Assert.Equal(response.HasException, false);
            Assert.Equal(response.HasWarning, false);
            Assert.Equal(response.HasInfo, true);
            Assert.Equal(response.BusinessInfos().First().Message, "There are no Payloads transmitted");
        }

        [Fact]
        public void PayloadTransportTest2WithFreshDataToBeTransportedWithRetries()
        {
            var businessData = GetDipsTransportBusinessData();

            var payloadConfig = Configuration.Transports.First();

            var dipsTransportDataAccess = GetDipsTransportDataAccessMock(true);
            var dipsTransportZipCreator = GetDipsTransportZipCreatorMock();
            var dipsTransportPgpCreator = GetDipsTransportPgpCreatorMock();
            var fileSystem = GetFileSystemMock();

            var pathToSftpTransporter = new Mock<PathToSftpTransportProcessor>(payloadConfig);

            pathToSftpTransporter.Setup(x => x.Transport(It.IsAny<DipsTransportBusinessData>()))
                .Returns(new DipsTransportBusinessResult());

            var payloadTransportProcessor = new DefaultPayloadTransportProcessor(
                pathToSftpTransporter.Object,
                dipsTransportDataAccess,
                dipsTransportZipCreator,
                dipsTransportPgpCreator,
                fileSystem
            );

            var response = payloadTransportProcessor.Transport(businessData);

            Assert.Equal(response.HasException, false);
            Assert.Equal(response.HasWarning, false);
            Assert.Equal(response.HasInfo, true);
            Assert.Equal(response.BusinessInfos().First().Message, "Payload transmission successful (total 2), TransmissionId = 4, FileName = DPSE-FXA-PAYLOAD-000000004.ZIP.PGP, Retries = 2 | Payload transmission successful (total 2), TransmissionId = 5, FileName = DPSE-FXA-PAYLOAD-000000005.ZIP.PGP, Retries = 0");
        }

        [Fact]
        public void PayloadTransportTest2WithDataToBeTransportedWithRetries()
        {
            var businessData = GetDipsTransportBusinessData();

            var payloadConfig = Configuration.Transports.First();

            var dipsTransportDataAccess = GetDipsTransportDataAccessMock(true);
            var dipsTransportZipCreator = GetDipsTransportZipCreatorMock();
            var dipsTransportPgpCreator = GetDipsTransportPgpCreatorMock();
            var fileSystem = GetFileSystemMock();

            var pathToSftpTransporter = new Mock<PathToSftpTransportProcessor>(payloadConfig);

            pathToSftpTransporter.Setup(x => x.Transport(It.IsAny<DipsTransportBusinessData>()))
                .Returns(new DipsTransportBusinessResult());

            var payloadTransportProcessor = new DefaultPayloadTransportProcessor(
                pathToSftpTransporter.Object,
                dipsTransportDataAccess,
                dipsTransportZipCreator,
                dipsTransportPgpCreator,
                fileSystem
            );

            var response = payloadTransportProcessor.Transport(businessData);

            Assert.Equal(response.HasException, false);
            Assert.Equal(response.HasWarning, false);
            Assert.Equal(response.HasInfo, true);
            Assert.Equal(response.BusinessInfos().First().Message, "Payload transmission successful (total 2), TransmissionId = 4, FileName = DPSE-FXA-PAYLOAD-000000004.ZIP.PGP, Retries = 2 | Payload transmission successful (total 2), TransmissionId = 5, FileName = DPSE-FXA-PAYLOAD-000000005.ZIP.PGP, Retries = 0");
        }


        [Fact]
        public void PayloadTransportTest2WithDataToBeTransportedWithRetriesExceptionInPathToSftpProcessor()
        {
            var businessData = GetDipsTransportBusinessData();

            var payloadConfig = Configuration.Transports.First();

            var dipsTransportDataAccess = GetDipsTransportDataAccessMock(true);
            var dipsTransportZipCreator = GetDipsTransportZipCreatorMock();
            var dipsTransportPgpCreator = GetDipsTransportPgpCreatorMock();
            var fileSystem = GetFileSystemMock();

            var pathToSftpTransporter = new Mock<PathToSftpTransportProcessor>(payloadConfig);

            pathToSftpTransporter.Setup(x => x.Transport(It.IsAny<DipsTransportBusinessData>()))
                .Returns(() =>
                    {
                        var result = new DipsTransportBusinessResult();
                        result.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException, "DPSE-4004", "SFPT Failed", string.Empty));
                        return result;
                    }
                );

            var payloadTransportProcessor = new DefaultPayloadTransportProcessor(
                pathToSftpTransporter.Object,
                dipsTransportDataAccess,
                dipsTransportZipCreator,
                dipsTransportPgpCreator,
                fileSystem
            );

            var response = payloadTransportProcessor.Transport(businessData);

            Assert.Equal(response.HasException, false);
            Assert.Equal(response.HasWarning, false);
            Assert.Equal(response.HasInfo, true);
            Assert.Equal(response.BusinessInfos().First().Message, "There are no Payloads transmitted");
        }
    }
}