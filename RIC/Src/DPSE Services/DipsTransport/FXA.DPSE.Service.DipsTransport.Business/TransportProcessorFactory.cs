using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.Business.EodTransport;
using FXA.DPSE.Service.DipsTransport.Common.Configuration;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;
using FXA.DPSE.Service.DipsTransport.Business.PayloadTransport;
using FXA.DPSE.Service.DipsTransport.Business.SimpleTransport;
using FXA.DPSE.Service.DipsTransport.DataAccess;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public class TransportProcessorFactory : ITransportProcessorFactory
    {
        private const string DipsPayloadTransportElementName = "dipsPayload";
        private const string DipsEodTransportElementName = "dipsEod";

        private readonly TransportElement _dipsPayloadTransportElement;
        private readonly TransportElement _dipsEodTransportElement;

        private readonly IDipsTransportDataAccess _dataAccess;
        private readonly IDipsTransportServiceConfiguration _configiguration;
        private readonly IDipsTransportMetadataCreator _metadataCreator;
        private readonly IDipsTransportZipCreator _zipCreator;
        private readonly IDipsTransportPgpCreator _pgpCreator;
        private readonly IFileSystem _fileSystem;

        public TransportProcessorFactory(IDipsTransportServiceConfiguration configiguration, IDipsTransportDataAccess dataAccess, IDipsTransportMetadataCreator metadataCreator, IDipsTransportZipCreator zipCreator, IDipsTransportPgpCreator pgpCreator, IFileSystem fileSystem)
        {
            var dipsPayloadTransportElement = configiguration
                .Transports
                .FirstOrDefault(t => t.Name.Equals(DipsPayloadTransportElementName, StringComparison.CurrentCultureIgnoreCase));

            var dipsEodTransportElement = configiguration
                .Transports
                .FirstOrDefault(t => t.Name.Equals(DipsEodTransportElementName, StringComparison.CurrentCultureIgnoreCase));

            if (dipsPayloadTransportElement == null)
                throw new ConfigurationErrorsException(string.Format("Transport configuration with element named '{0}' not configured in web.config file (i.e. <serviceConfig> <transports> <transport name={1}{0}{1}> </transport> </transports> </serviceConfig> is not configured).", DipsPayloadTransportElementName, "\""));

            if (dipsEodTransportElement == null)
                throw new ConfigurationErrorsException(string.Format("Transport configuration with element named '{0}' not configured in web.config file (i.e. <serviceConfig> <transports> <transport name={1}{0}{1}> </transport> </transports> </serviceConfig> is not configured).", DipsEodTransportElementName, "\""));

            _dipsPayloadTransportElement = dipsPayloadTransportElement;
            _dipsEodTransportElement = dipsEodTransportElement;
            _configiguration = configiguration;
            _dataAccess = dataAccess;
            _metadataCreator = metadataCreator;
            _zipCreator = zipCreator;
            _pgpCreator = pgpCreator;
            _fileSystem = fileSystem;
        }

        public ITransportProcessor GetPayloadTransporter()
        {
            return new DefaultPayloadTransportProcessor(GetSimpleTransporterForPayload(), _dataAccess, _zipCreator, _pgpCreator, _fileSystem);
        }

        public ITransportProcessor GetEodTransporter()
        {
            return new DefaultEodTransportProcessor(GetSimpleTransporterForEod(), _dataAccess, _metadataCreator, _zipCreator, _pgpCreator);
        }

        private ITransportProcessor GetSimpleTransporterForPayload()
        {
            if (_dipsPayloadTransportElement.Source.Type == SourceTypes.Path && _dipsPayloadTransportElement.Destination.Type == DestinationTypes.Path)
            {
                return new PathToPathTransportProcessor(_dipsPayloadTransportElement);
            }

            if (_dipsPayloadTransportElement.Source.Type == SourceTypes.Path && _dipsPayloadTransportElement.Destination.Type == DestinationTypes.Sftp)
            {
                return new PathToSftpTransportProcessor(_dipsPayloadTransportElement);
            }

            if (_dipsPayloadTransportElement.Source.Type == SourceTypes.Sftp && _dipsPayloadTransportElement.Destination.Type == DestinationTypes.Path)
            {
                return new SftpToPathTransportProcessor(_dipsPayloadTransportElement);
            }

            if (_dipsPayloadTransportElement.Source.Type == SourceTypes.Sftp && _dipsPayloadTransportElement.Destination.Type == DestinationTypes.Sftp)
            {
                return new SftpToSftpTransportProcessor(_dipsPayloadTransportElement);
            }

            throw new ConfigurationErrorsException(string.Format(""));
        }

        private ITransportProcessor GetSimpleTransporterForEod()
        {
            if (_dipsEodTransportElement.Source.Type == SourceTypes.Path && _dipsEodTransportElement.Destination.Type == DestinationTypes.Path)
            {
                return new PathToPathTransportProcessor(_dipsEodTransportElement);
            }

            if (_dipsEodTransportElement.Source.Type == SourceTypes.Path && _dipsEodTransportElement.Destination.Type == DestinationTypes.Sftp)
            {
                return new PathToSftpTransportProcessor(_dipsEodTransportElement);
            }

            if (_dipsEodTransportElement.Source.Type == SourceTypes.Sftp && _dipsEodTransportElement.Destination.Type == DestinationTypes.Path)
            {
                return new SftpToPathTransportProcessor(_dipsEodTransportElement);
            }

            if (_dipsEodTransportElement.Source.Type == SourceTypes.Sftp && _dipsEodTransportElement.Destination.Type == DestinationTypes.Sftp)
            {
                return new SftpToSftpTransportProcessor(_dipsEodTransportElement);
            }

            throw new ConfigurationErrorsException(string.Format(""));
        }
    }
}