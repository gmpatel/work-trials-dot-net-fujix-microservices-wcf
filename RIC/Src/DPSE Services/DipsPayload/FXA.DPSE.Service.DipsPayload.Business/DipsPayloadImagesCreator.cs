using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FXA.DPSE.Framework.Common.Exception;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DipsPayload.Business.Core;
using FXA.DPSE.Service.DipsPayload.Common.Configuration;
using FXA.DPSE.Service.DipsPayload.Common.Extensions;
using FXA.DPSE.Service.DipsPayload.Common.PayloadException;
using FXA.DPSE.Service.DipsPayload.DataAccess;

namespace FXA.DPSE.Service.DipsPayload.Business
{
    public class DipsPayloadImagesCreator : IDipsPayloadImagesCreator
    {
        private readonly IDipsPayloadServiceConfiguration _serviceConfiguration;
        private readonly IAuditProxy _auditProxy;

        public DipsPayloadImagesCreator(IDipsPayloadServiceConfiguration serviceConfiguration, IAuditProxy auditProxy)
        {
            _serviceConfiguration = serviceConfiguration;
            _auditProxy = auditProxy;
        }

        public bool GeneratePayloadImages(PaymentInstruction paymentInstruction, DirectoryInfo directory)
        {

            if (paymentInstruction.ChequeCount <= 0) return false;
            foreach (var voucher in paymentInstruction.Vouchers)
            {
                if (voucher.IsNonPostingCheque) continue;

                CreateFrontChequeImages(directory, voucher, paymentInstruction);
                CreateRearChequeImages(directory, voucher, paymentInstruction);
            }
            return true;
        }

        private void CreateRearChequeImages(DirectoryInfo directory, Voucher voucher, PaymentInstruction paymentInstruction)
        {
            if (string.IsNullOrEmpty(voucher.VoucherImage.RearImage)) return;

            var rearImageBase64 = voucher.VoucherImage.RearImage;

            var rearImageJpegByte = Convert.FromBase64String(rearImageBase64);
            var rearJpegImageFileName = string.Format("VOUCHER_{0}_{1}_REAR.JPG", DateTime.UtcNow.ToString("ddMMyyyy"),
                string.Format("{0}{1}", _serviceConfiguration.PayloadProcessingDetails.DocumentReferenceNumberPreFix,
                    voucher.TrackingId.GetLast(6)));
            var rearJpegImageFilePath = Path.Combine(directory.FullName, rearJpegImageFileName);

            byte[] rearImageTiffBytes;
            var rearTiffImageFileName = string.Format("VOUCHER_{0}_{1}_REAR.TIF", DateTime.UtcNow.ToString("ddMMyyyy"),
                string.Format("{0}{1}", _serviceConfiguration.PayloadProcessingDetails.DocumentReferenceNumberPreFix,
                    voucher.TrackingId.GetLast(6)));
            var rearTiffImageFilePath = Path.Combine(directory.FullName, rearTiffImageFileName);

            using (var inStream = new MemoryStream(rearImageJpegByte))
            {
                using (var outStream = new MemoryStream())
                {
                    Image.FromStream(inStream).Save(outStream, ImageFormat.Tiff);
                    rearImageTiffBytes = outStream.ToArray();
                }
            }

            try
            {
                if (!File.Exists(rearJpegImageFilePath))
                    File.WriteAllBytes(rearJpegImageFilePath, rearImageJpegByte);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.FileAccessError,
                    exception.Message, exception, string.Empty);
            }
            

            var rearJpegImageAuditResult = _auditProxy.AuditAsync(voucher.TrackingId, "FileSystemWriteAccess",
               string.Format("Created Rear Jpeg Image {0} for payload", rearJpegImageFileName),
               string.Format("Location:{0}", directory.FullName), paymentInstruction.ChannelType,
               paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (rearJpegImageAuditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, rearJpegImageAuditResult.BusinessException.Message,
                    rearJpegImageAuditResult.BusinessException.ErrorCode);

            try
            {
                if (!File.Exists(rearTiffImageFilePath))
                    File.WriteAllBytes(rearTiffImageFilePath, rearImageTiffBytes);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.FileAccessError,
                    exception.Message, exception, string.Empty);
            }
            

            var rearImageAuditResult = _auditProxy.AuditAsync(voucher.TrackingId, "FileSystemWriteAccess",
             string.Format("Created Rear Jpeg Image {0} for payload", rearJpegImageFileName),
             string.Format("Location:{0}", directory.FullName), paymentInstruction.ChannelType,
             paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (rearImageAuditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, rearImageAuditResult.BusinessException.Message,
                    rearImageAuditResult.BusinessException.ErrorCode);

            var rearTiffImageAuditResult = _auditProxy.AuditAsync(voucher.TrackingId, "FileSystemWriteAccess",
               string.Format("Created Rear Jpeg Image {0} for payload", rearJpegImageFileName),
               string.Format("Location:{0}", directory.FullName), paymentInstruction.ChannelType,
               paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (rearTiffImageAuditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, rearTiffImageAuditResult.BusinessException.Message,
                    rearTiffImageAuditResult.BusinessException.ErrorCode);

        }

        private void CreateFrontChequeImages(DirectoryInfo directory, Voucher voucher, PaymentInstruction paymentInstruction)
        {
            if (string.IsNullOrEmpty(voucher.VoucherImage.FrontImage)) return;

            var frontImageBase64 = voucher.VoucherImage.FrontImage;

            var frontImageJpegByte = Convert.FromBase64String(frontImageBase64);
            var frontJpegImageFileName = string.Format("VOUCHER_{0}_{1}_FRONT.JPG", DateTime.UtcNow.ToString("ddMMyyyy"),
                string.Format("{0}{1}", _serviceConfiguration.PayloadProcessingDetails.DocumentReferenceNumberPreFix,
                    voucher.TrackingId.GetLast(6)));

            var frontJpegImageFilePath = Path.Combine(directory.FullName, frontJpegImageFileName);

            byte[] frontImageTiffBytes;
            var frontTiffImageFileName = string.Format("VOUCHER_{0}_{1}_FRONT.TIF", DateTime.UtcNow.ToString("ddMMyyyy"),
                string.Format("{0}{1}", _serviceConfiguration.PayloadProcessingDetails.DocumentReferenceNumberPreFix,
                    voucher.TrackingId.GetLast(6)));

            var frontTiffImageFilePath = Path.Combine(directory.FullName, frontTiffImageFileName);
            using (var inStream = new MemoryStream(frontImageJpegByte))
            {
                using (var outStream = new MemoryStream())
                {
                    Image.FromStream(inStream).Save(outStream, ImageFormat.Tiff);
                    frontImageTiffBytes = outStream.ToArray();
                }
            }

            try
            {
                if (!File.Exists(frontJpegImageFilePath))
                    File.WriteAllBytes(frontJpegImageFilePath, frontImageJpegByte);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.FileAccessError,
                    exception.Message, exception, string.Empty);
            }
            

            var frontJpegImageAuditResult = _auditProxy.AuditAsync(voucher.TrackingId, "FileSystemWriteAccess",
                string.Format("Created Front Jpeg Image {0} for payload", frontJpegImageFileName),
                string.Format("Location:{0}", directory.FullName), paymentInstruction.ChannelType,
                paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (frontJpegImageAuditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, frontJpegImageAuditResult.BusinessException.Message,
                    frontJpegImageAuditResult.BusinessException.ErrorCode);

            try
            {
                if (!File.Exists(frontTiffImageFilePath))
                    File.WriteAllBytes(frontTiffImageFilePath, frontImageTiffBytes);
            }
            catch (Exception exception)
            {
                throw new ProcessingException<DipsPayloadErrorType>(DipsPayloadErrorType.FileAccessError,
                    exception.Message, exception, string.Empty);
            }

            var rearTiffImageAuditResult = _auditProxy.AuditAsync(voucher.TrackingId, "FileSystemWriteAccess",
               string.Format("Created Front Tiff Image {0} for payload", frontTiffImageFileName),
               string.Format("Location:{0}", directory.FullName), paymentInstruction.ChannelType,
               paymentInstruction.ClientSession.SessionId, Environment.MachineName, "DipsPayload", "DipsPayloadBatch");

            if (rearTiffImageAuditResult.HasException)
                throw new ProcessingException<ProxyError>(ProxyError.AuditError, rearTiffImageAuditResult.BusinessException.Message,
                    rearTiffImageAuditResult.BusinessException.ErrorCode);
        }
    }
}