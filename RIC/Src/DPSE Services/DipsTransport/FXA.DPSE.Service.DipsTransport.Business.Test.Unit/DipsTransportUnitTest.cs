using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.EodTransport;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Business.PayloadTransport;
using FXA.DPSE.Service.DipsTransport.DataAccess;
using Moq;
using Xunit;

namespace FXA.DPSE.Service.DipsTransport.Business.Test.Unit
{
    public class DipsTransportUnitTest
    {
        public IDipsTransportServiceConfiguration Configuration1 { get; private set; }
        public IDipsTransportServiceConfiguration Configuration2 { get; private set; }
        public IDipsTransportServiceConfiguration Configuration3 { get; private set; }
        public IDipsTransportServiceConfiguration Configuration4 { get; private set; }

        public DipsTransportUnitTest()
        {
            var config1 = new Mock<IDipsTransportServiceConfiguration>();
            
            config1.SetupGet(prop => prop.Transports).Returns(new List<TransportElement>
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
                });

            Configuration1 = config1.Object;

            var config2 = new Mock<IDipsTransportServiceConfiguration>();

            config2.SetupGet(prop => prop.Transports).Returns(new List<TransportElement>
                {
                    new TransportElement
                    {
                        Name = "dipsPayload",
                        Source = new SourceElement
                        {
                            Type = SourceTypes.Path,
                            Path = @"C:\Temp\Shared\DipsPayloads\Payloads",
                            RegEx = @".jpg|.tiff"
                        },
                        Destination = new DestinationElement
                        {
                            Type = DestinationTypes.Path,
                            Path = @"C:\Temp\Shared\DipsPayloads\PayloadsProcessed",
                        },
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
                });

            Configuration2 = config2.Object;

            var config3 = new Mock<IDipsTransportServiceConfiguration>();

            config3.SetupGet(prop => prop.Transports).Returns(new List<TransportElement>
                {
                    new TransportElement
                    {
                        Name = "dipsPayload",
                        Source = new SourceElement
                        {
                            Type = SourceTypes.Sftp,
                            Path = @"/DipsPayloads",
                            RegEx = @".jpg|.tiff",
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
                        Destination = new DestinationElement
                        {
                            Type = DestinationTypes.Path,
                            Path = @"\\NRYE745-B31RY52\DipsPayloads\Processed",
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
                });

            Configuration3 = config3.Object;

            var config4 = new Mock<IDipsTransportServiceConfiguration>();

            config4.SetupGet(prop => prop.Transports).Returns(new List<TransportElement>
                {
                    new TransportElement
                    {
                        Name = "dipsPayload",
                        Source = new SourceElement
                        {
                            Type = SourceTypes.Sftp,
                            Path = @"/DipsPayloads",
                            RegEx = @".jpg|.tiff",
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
                        Destination = new DestinationElement
                        {
                            Type = DestinationTypes.Sftp,
                            Path = @"/DipsPayloadsProcessed",
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
                });

            Configuration4 = config4.Object;
        }

        [Fact]
        public void verify_factory_returns_defaultpayloadtransportprocessor_with_pathtosftptransportprocessor_on_configuration1()
        {
            var dipsTransportDataAccess = new Mock<IDipsTransportDataAccess>();
            var dipsTransportMetadataCreator = new Mock<IDipsTransportMetadataCreator>();
            var dipsTransportZipCreator = new Mock<IDipsTransportZipCreator>();
            var dipsTransportPgpCreator = new Mock<IDipsTransportPgpCreator>();
            var fileSystem = new Mock<IFileSystem>();

            var factory = new TransportProcessorFactory(Configuration1, 
                dipsTransportDataAccess.Object, 
                dipsTransportMetadataCreator.Object, 
                dipsTransportZipCreator.Object, 
                dipsTransportPgpCreator.Object,
                fileSystem.Object
            );
            
            var transporter = factory.GetPayloadTransporter();

            Assert.Equal(transporter.GetType().FullName,
                "FXA.DPSE.Service.DipsTransport.Business.PayloadTransport.DefaultPayloadTransportProcessor");

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Source.Type,
                SourceTypes.Path);

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Destination.Type, 
                DestinationTypes.Sftp);
        }

        [Fact]
        public void verify_factory_returns_defaultpayloadtransportprocessor_with_pathtopathtransportprocessor_on_configuration2()
        {
            var dipsTransportDataAccess = new Mock<IDipsTransportDataAccess>();
            var dipsTransportMetadataCreator = new Mock<IDipsTransportMetadataCreator>();
            var dipsTransportZipCreator = new Mock<IDipsTransportZipCreator>();
            var dipsTransportPgpCreator = new Mock<IDipsTransportPgpCreator>();
            var fileSystem = new Mock<IFileSystem>();

            var factory = new TransportProcessorFactory(Configuration2,
                dipsTransportDataAccess.Object,
                dipsTransportMetadataCreator.Object,
                dipsTransportZipCreator.Object,
                dipsTransportPgpCreator.Object,
                fileSystem.Object
            );

            var transporter = factory.GetPayloadTransporter();

            Assert.Equal(transporter.GetType().FullName,
                "FXA.DPSE.Service.DipsTransport.Business.PayloadTransport.DefaultPayloadTransportProcessor");

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Source.Type,
                SourceTypes.Path);

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Destination.Type,
                DestinationTypes.Path);
        }

        [Fact]
        public void verify_factory_returns_defaultpayloadtransportprocessor_with_sftptopathtransportprocessor_on_configuration3()
        {
            var dipsTransportDataAccess = new Mock<IDipsTransportDataAccess>();
            var dipsTransportMetadataCreator = new Mock<IDipsTransportMetadataCreator>();
            var dipsTransportZipCreator = new Mock<IDipsTransportZipCreator>();
            var dipsTransportPgpCreator = new Mock<IDipsTransportPgpCreator>();
            var fileSystem = new Mock<IFileSystem>();

            var factory = new TransportProcessorFactory(Configuration3,
                dipsTransportDataAccess.Object,
                dipsTransportMetadataCreator.Object,
                dipsTransportZipCreator.Object,
                dipsTransportPgpCreator.Object,
                fileSystem.Object
            ); 
            
            var transporter = factory.GetPayloadTransporter();

            Assert.Equal(transporter.GetType().FullName,
                "FXA.DPSE.Service.DipsTransport.Business.PayloadTransport.DefaultPayloadTransportProcessor");

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Source.Type,
                SourceTypes.Sftp);

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Destination.Type, 
                DestinationTypes.Path);
        }

        [Fact]
        public void verify_factory_returns_defaultpayloadtransportprocessor_with_sftptosftptransportprocessor_on_configuration4()
        {
            var dipsTransportDataAccess = new Mock<IDipsTransportDataAccess>();
            var dipsTransportMetadataCreator = new Mock<IDipsTransportMetadataCreator>();
            var dipsTransportZipCreator = new Mock<IDipsTransportZipCreator>();
            var dipsTransportPgpCreator = new Mock<IDipsTransportPgpCreator>();
            var fileSystem = new Mock<IFileSystem>();

            var factory = new TransportProcessorFactory(Configuration4,
                dipsTransportDataAccess.Object,
                dipsTransportMetadataCreator.Object,
                dipsTransportZipCreator.Object,
                dipsTransportPgpCreator.Object,
                fileSystem.Object
            ); 

            var transporter = factory.GetPayloadTransporter();

            Assert.Equal(transporter.GetType().FullName,
                "FXA.DPSE.Service.DipsTransport.Business.PayloadTransport.DefaultPayloadTransportProcessor");

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Source.Type,
                SourceTypes.Sftp);

            Assert.Equal(((DefaultPayloadTransportProcessor)transporter).Destination.Type,
                DestinationTypes.Sftp);
        }

        [Fact]
        public void verify_pathtopathtransportprocessor()
        {
            var dipsTransportDataAccess = new Mock<IDipsTransportDataAccess>();
            var dipsTransportMetadataCreator = new Mock<IDipsTransportMetadataCreator>();
            var dipsTransportZipCreator = new Mock<IDipsTransportZipCreator>();
            var dipsTransportPgpCreator = new Mock<IDipsTransportPgpCreator>();
            var fileSystem = new Mock<IFileSystem>();

            var factory = new TransportProcessorFactory(Configuration1,
                dipsTransportDataAccess.Object,
                dipsTransportMetadataCreator.Object,
                dipsTransportZipCreator.Object,
                dipsTransportPgpCreator.Object,
                fileSystem.Object
            );

            var transporter = factory.GetEodTransporter();

            Assert.Equal(transporter.GetType().FullName,
                "FXA.DPSE.Service.DipsTransport.Business.EodTransport.DefaultEodTransportProcessor");

            Assert.Equal(((DefaultEodTransportProcessor)transporter).Source.Type,
                SourceTypes.Path);

            Assert.Equal(((DefaultEodTransportProcessor)transporter).Destination.Type,
                DestinationTypes.Sftp);
        }
    }
}