using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Service.DipsTransport.Business.Core;
using FXA.DPSE.Service.DipsTransport.DataAccess;
using FXA.DPSE.Service.DipsTransport.Business.Entities;
using FXA.DPSE.Service.DipsTransport.Business.Entities.Serializable;
using System.Globalization;
using System.IO;
using FXA.DPSE.Service.DipsTransport.Common.Helper;
using FXA.DPSE.Framework.Common.Security.PGP;
using FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Business.EodTransport
{
    public class DefaultEodTransportProcessor : BaseTransportProcessor
    {
        private readonly BaseTransportProcessor _transporter;
        private readonly IDipsTransportDataAccess _dataAccess;
        private readonly IDipsTransportMetadataCreator _metadataCreator;
        private readonly IDipsTransportZipCreator _zipCreator;
        private readonly IDipsTransportPgpCreator _pgpCreator;

        public DefaultEodTransportProcessor(
            ITransportProcessor transporter, 
            IDipsTransportDataAccess dataAccess,
            IDipsTransportMetadataCreator metadataCreator,
            IDipsTransportZipCreator zipCreator,
            IDipsTransportPgpCreator pgpCreator) 
        : base(((BaseTransportProcessor)transporter).Configuration)
        {
            this._transporter = (BaseTransportProcessor) transporter;
            this._dataAccess = dataAccess;
            this._metadataCreator = metadataCreator;
            this._zipCreator = zipCreator;
            this._pgpCreator = pgpCreator;
        }

        public override DipsTransportBusinessResult Transport(DipsTransportBusinessData data)
        {
            DipsTransportBusinessResult businessResult;
 
            try
            {
                DateTime reportDate;
                
                if (DateTime.TryParseExact(data.ReportDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out reportDate))
                {
                    var transmissions = _dataAccess.GetTransportedTransmissions(reportDate);
                    var targetDirectory = new DirectoryInfo(base.Configuration.TempLocation.Path);

                    var xmlFile = _metadataCreator.GenerateMetadata(
                            reportDate,
                            transmissions,
                            targetDirectory
                        );

                    if (xmlFile != null && xmlFile.Exists)
                    {
                        //var targetFiles = new List<FileInfo> { xmlFile };
                        //var eodZipFileName = string.Format("{0}.ZIP", xmlFile.Name);
                        //var eodZipFilePath = Path.Combine(targetDirectory.FullName, eodZipFileName);

                        //var eodZipFile = _zipCreator.CreateZip(new FileInfo(eodZipFilePath), targetFiles);

                        var eodPgpFileName = string.Format("{0}.PGP", xmlFile);
                        var eodPgpFilePath = Path.Combine(targetDirectory.FullName, eodPgpFileName);
                        var eodPgpFile = new FileInfo(eodPgpFilePath);

                        if (eodPgpFile.Exists) eodPgpFile.Delete();

                        eodPgpFile = _pgpCreator.CreatePgp(xmlFile);

                        _transporter.Source = new SourceElement
                        {
                            Path = eodPgpFile.DirectoryName,
                            RegEx = eodPgpFile.Name
                        };

                        businessResult = _transporter.Transport(data);
                        return businessResult;
                    }

                    businessResult = new DipsTransportBusinessResult();
                    businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException, "DPSE-4003", "Unable to generate EOD xml file.", string.Empty));
                    return businessResult;
                }

                businessResult = new DipsTransportBusinessResult();
                businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.RequestValidation, "DPSE-4001", "Invalid ReportDate format. Please provide report date in YYYY-MM-DD format.", string.Empty));
                return businessResult;
            }
            catch (Exception exception)
            {
                businessResult = new DipsTransportBusinessResult();
                businessResult.AddBusinessException(new DpseBusinessException(DpseBusinessExceptionType.ApplicationException, "DPSE-4003", exception.Message, string.Empty));
                return businessResult;
            }
        }
    }
}
