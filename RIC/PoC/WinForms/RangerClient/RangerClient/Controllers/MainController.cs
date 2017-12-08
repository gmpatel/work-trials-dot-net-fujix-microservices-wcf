using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using AxRANGERLib;
using FujiXerox.RangerClient.Enums;
using FujiXerox.RangerClient.Models;
using FujiXerox.RangerClient.Views;
using Serilog;

namespace FujiXerox.RangerClient.Controllers
{
    public class MainController
    {
        private readonly IMainView _mainView;
        private readonly Dictionary<int, Voucher> _vouchers;
        private const string IMAGEPATH = @"C:\_Images";
        private const string RANGERIMAGEPATH = @"C:\_Images";

        public MainController(IMainView mainView)
        {
            Log.Debug("MainController: Constructor()");
            _mainView = mainView;
            _vouchers = new Dictionary<int, Voucher>();
            Initialise();
        }

        private void Initialise()
        {
            Log.Debug("MainController: Initialise()");
            SubscribeToRangerEvents();
            var status = RangerTransportState.TransportShutDown;
            _mainView.ScannerStatus = status.ToString();
            _mainView.StartUpEnabled = true;
            _mainView.ShutDownEnabled =
                _mainView.EnableOptionsEnabled =
                    _mainView.StartFeedingEnabled =
                        _mainView.StopFeedingEnabled =
                            _mainView.PrepareToChangeOptionsEnabled = false;
        }

        private void SubscribeToRangerEvents()
        {
            Log.Debug("MainController: SubscribeToRangerEvents()");
            _mainView.AxRanger.TransportNewState += AxRanger_TransportNewState;
            _mainView.AxRanger.TransportChangeOptionsState += AxRanger_TransportChangeOptionsState;
            _mainView.AxRanger.TransportFeedingStopped += AxRanger_TransportFeedingStopped;
            _mainView.AxRanger.TransportNewItem += AxRanger_TransportNewItem;
            _mainView.AxRanger.TransportSetItemOutput += AxRanger_TransportSetItemOutput;
            _mainView.AxRanger.TransportItemInPocket += AxRanger_TransportItemInPocket;
        }

        private void UnsubscribeToRangerEvents()
        {
            Log.Debug("MainController: UnsubscribeToRangerEvents()");
            _mainView.AxRanger.TransportNewState -= AxRanger_TransportNewState;
            _mainView.AxRanger.TransportChangeOptionsState -= AxRanger_TransportChangeOptionsState;
            _mainView.AxRanger.TransportFeedingStopped -= AxRanger_TransportFeedingStopped;
            _mainView.AxRanger.TransportNewItem -= AxRanger_TransportNewItem;
            _mainView.AxRanger.TransportSetItemOutput -= AxRanger_TransportSetItemOutput;
            _mainView.AxRanger.TransportItemInPocket -= AxRanger_TransportItemInPocket;
        }

        private void AxRanger_TransportItemInPocket(object sender,
            _DRangerEvents_TransportItemInPocketEvent e)
        {
            Log.Debug("MainController: Event AxRanger_TransportItemInPocket");
            _mainView.EventStatus = string.Format("Item {0} TransportItemInPocket", e.itemId);
            GetTransportItem(e.itemId);
        }

        private void AxRanger_TransportSetItemOutput(object sender,
            _DRangerEvents_TransportSetItemOutputEvent e)
        {
            Log.Debug("MainController: Event AxRanger_TransportSetItemOutput");
            _mainView.EventStatus = string.Format("Item {0} TransportSetItemOutput", e.itemId);
        }

        private void AxRanger_TransportNewItem(object sender, EventArgs e)
        {
            Log.Debug("MainController: Event AxRanger_TransportNewItem");
            _mainView.EventStatus = "TransportNewItem";
        }

        private void AxRanger_TransportFeedingStopped(object sender,
            _DRangerEvents_TransportFeedingStoppedEvent e)
        {
            Log.Debug("MainController: Event AxRanger_TransportFeedingStopped");
            _mainView.EventStatus = "TransportFeedingStopped";
        }

        private void AxRanger_TransportChangeOptionsState(object sender,
            _DRangerEvents_TransportChangeOptionsStateEvent e)
        {
            Log.Debug("MainController: Event AxRanger_TranportChangeOptionsState");
            _mainView.EventStatus = "TransportChangedOptionsState";
        }

        private void AxRanger_TransportNewState(object sender, _DRangerEvents_TransportNewStateEvent e)
        {
            Log.Debug("MainController: Event AxRanger_TransportNewState");
            var current = e.currentState;
            var status = (RangerTransportState) current;
            _mainView.ScannerStatus = status.ToString();
            _mainView.StartUpEnabled = status == RangerTransportState.TransportShutDown;
            _mainView.ShutDownEnabled = status == RangerTransportState.TransportChangeOptions ||
                                        status == RangerTransportState.TransportReadyToFeed;
            _mainView.EnableOptionsEnabled = status == RangerTransportState.TransportChangeOptions;
            _mainView.StartFeedingEnabled = status == RangerTransportState.TransportReadyToFeed;
            _mainView.StopFeedingEnabled = status == RangerTransportState.TransportFeeding ||
                                           status == RangerTransportState.TransportExceptionInProgress;
            _mainView.PrepareToChangeOptionsEnabled = status == RangerTransportState.TransportReadyToFeed;
        }

        public void StartUp()
        {
            Log.Debug("MainController: StartUp()");
            _mainView.AxRanger.StartUp();
        }

        public void ShutDown()
        {
            Log.Debug("MainController: ShutDown()");
            _mainView.AxRanger.ShutDown();
        }

        public void StartFeeding()
        {
            Log.Debug("MainController: StartFeeding()");
            _mainView.AxRanger.StartFeeding(0, 0);
        }

        public void StopFeeding()
        {
            Log.Debug("MainController: StopFeeding()");
            _mainView.AxRanger.StopFeeding();
        }

        public void EnableOptions()
        {
            Log.Debug("MainController: EnableOptions()");
            SetOptionsLogging(true);
            //SetImagingOptions();
            //SetImageDirectory();
            _mainView.AxRanger.EnableOptions();
        }

        public void ChangeOptions()
        {
            Log.Debug("MainController: ChangeOptions()");
            if (GetTransportInfo("General", "IQAAvailableDuringOutputEvent") != "true") return;

            //To do IQA testing we need to enable imaging
            SetGenericOption("OptionalDevices", "NeedImaging", "True");
            SetGenericOption("OptionalDevices", "NeedFrontImage1", "True");
            SetGenericOption("OptionalDevices", "NeedRearImage1", "True");

            //Turn off the images we aren't going to test
            SetGenericOption("OptionalDevices", "NeedFrontImage2", "False");
            SetGenericOption("OptionalDevices", "NeedRearImage2", "False");

            //Turn on IQA, then turn on IQA in the upstream(the setitemoutput) event 
            SetGenericOption("OptionalDevices", "NeedIQA", "True");
            SetGenericOption("OptionalDevices", "NeedIQAUpstream", "True");
        }

        private void SetOptionsLogging(bool value)
        {
            SetGenericOption("OptionsLogging", "Enabled", value ? "True" : "False");
        }

        private void SetImagingOptions()
        {
            Log.Debug("MainController: SetImagingOptions()");
            //enable imaging
            SetGenericOption("OptionalDevices", "NeedImaging", "True");
            SetGenericOption("OptionalDevices", "NeedFrontImage1", "True");
            SetGenericOption("OptionalDevices", "NeedRearImage1", "True");

            //Turn off the images we aren't going to use
            SetGenericOption("OptionalDevices", "NeedFrontImage2", "True");
            SetGenericOption("OptionalDevices", "NeedRearImage2", "True");
            SetGenericOption("OptionalDevices", "NeedFrontImage3", "False");
            SetGenericOption("OptionalDevices", "NeedRearImage3", "False");
            SetGenericOption("OptionalDevices", "NeedFrontImage4", "False");
            SetGenericOption("OptionalDevices", "NeedRearImage4", "False");

            //SetGenericOption("FrontImage1", "StorageFile", "FIM");
            //SetGenericOption("FrontImage2", "StorageFile", "FIM");
            //SetGenericOption("FrontImage3", "StorageFile", "FI2");
            //SetGenericOption("RearImage1", "StorageFile", "RIM");
            //SetGenericOption("RearImage2", "StorageFile", "RIM");
            //SetGenericOption("RearImage3", "StorageFile", "RI2");
        }

        private void SetImageDirectory()
        {
            Log.Debug("MainController: SetImageDirectory()");
            var path = Path.Combine(IMAGEPATH, "Ranger");
            SetGenericOption("Imaging", "RootCaptureDirectory", path);
            var result = _mainView.AxRanger.SetImageFileSetDirectory("_IMG");
            Log.Debug("MainController: SetImageDirectory - SetImageFileSetDirectory({0}) is {1}", path, result);
        }

        private bool SetGenericOption(string sectionName, string valueName, string value)
        {
            var result = _mainView.AxRanger.SetGenericOption(sectionName, valueName, value);
            Log.Debug("MainController: SetGenericOption({0}, {1}, {2}) returns {3}", sectionName, valueName, value, result);
            return result;
        }

        public void PrepareToChangeOptions()
        {
            Log.Debug("MainController: PrepareToChangeOptions()");
            _mainView.AxRanger.PrepareToChangeOptions();
        }

        public void Unsubscribe()
        {
            Log.Debug("MainController: Unsubscribe()");
            UnsubscribeToRangerEvents();
        }

        public void SelectRow(int index)
        {
            Log.Debug("MainController: SelectRow({0})", index);
            if (_vouchers.Count < index) return;
            var voucher = _vouchers[index + 1];
            _mainView.FrontImage = voucher.FrontImage;
            _mainView.RearImage = voucher.RearImage;
        }

        public string GetTransportInfo(string sectionName, string valueName)
        {
            var result = _mainView.AxRanger.GetTransportInfo(sectionName, valueName);
            Log.Debug("MainController: GetTransportInfo({0}, {1}) returns {2}", sectionName, valueName, result);
            return result;
        }

        private void GetTransportItem(int index)
        {
            Log.Debug("MainController: GetTransportItem({0})", index);
            var micrText = _mainView.AxRanger.GetMicrText(1);
            var ocrText = _mainView.AxRanger.GetOcrText(1);
            var frontImage = GetVoucherImage(VoucherImageSide.Front, VoucherImageColourType.Grayscale);
            var rearImage = GetVoucherImage(VoucherImageSide.Rear, VoucherImageColourType.Grayscale);
            var voucher = new Voucher
            {
                VoucherId = index,
                FrontImage = frontImage,
                RearImage = rearImage,
                MicrText = micrText,
                OcrText = ocrText
            };
            _vouchers.Add(index, voucher);
            _mainView.FrontImage = frontImage;
            _mainView.RearImage = rearImage;
            _mainView.DataRow = new List<string>{index.ToString(), micrText, ocrText};
            const VoucherImageColourType colourType = VoucherImageColourType.Grayscale;
            var frontResult = SaveImageFile(VoucherImageSide.Front, index, (int)colourType, RANGERIMAGEPATH);
            var rearResult = SaveImageFile(VoucherImageSide.Rear, index, 2, RANGERIMAGEPATH);
            //SaveImage(frontImage, IMAGEPATH, index, VoucherImageSide.Front);
            //SaveImage(rearImage, IMAGEPATH, index, VoucherImageSide.Rear);
        }

        private bool SaveImageFile(VoucherImageSide side, int index, int colourType, string path)
        {
            var filename = Path.Combine(path, "Ranger", "_IMG",
                string.Format("{0}_{1}_{2}.jpg", index, DateTime.Now.ToString("yyyyMMdd"), side));
            Log.Debug("MainController: SaveImageFile(side = {0}, index = {1}, colourType = {2}, path = {3}) - filename = {4}", side, index, colourType, path, filename);

            // Image needs to be first selected with generic options
            var result = _mainView.AxRanger.SaveImageToFile((int)side, colourType, filename);
            Log.Debug("MainController: SaveImageFile(side = {0}, index = {1}, colourType = {2}, path = {3}) returns {4}", side, index, colourType, path, result);
            return result;
        }

        private void SaveImage(Image image, string path, int index, VoucherImageSide side)
        {
            var filename = Path.Combine(path,
                string.Format("{0}_{1}_{2}.jpg", index, DateTime.Now.ToString("yyyyMMdd"), side));
            Log.Debug("MainController: SaveImage(image = {0}, path = {1}, index = {2}, side = {3}) - filename = {4}", image, path, index, side, filename);
            if (image == null) return;
            using (var ms = new FileStream(filename, FileMode.CreateNew))
            {
                image.Save(ms, ImageFormat.Jpeg);
                //var encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(ms));
                //using (var filestream = new FileStream(filename, FileMode.Create))
                //    encoder.Save(filestream);
            }
        }

        private Image GetVoucherImage(VoucherImageSide transportSource, VoucherImageColourType imageColor)
        {
            Log.Debug("MainController: GetVoucherImage(transportSource = {0}, imageColor = {1})", transportSource, imageColor);
            var side = (int)transportSource;
            var colourType = (int)imageColor;
            var imageSize = _mainView.AxRanger.GetImageByteCount(side, colourType);
            if (imageSize == 0)
                return null;

            var imageBytes = new byte[imageSize];
            var ptr = new IntPtr(_mainView.AxRanger.GetImageAddress(side, colourType));

            Marshal.Copy(ptr, imageBytes, 0, imageSize);

            Image bitmap;

            using (var bitmapStream = new MemoryStream(imageBytes))
            {
                bitmap = new Bitmap(Image.FromStream(bitmapStream));
            }

            Log.Debug("MainController: GetVoucherImage(transportSource = {0}, imageColor = {1}) - imageSize = {2}", transportSource, imageColor, imageSize);

            return bitmap;
        }

    }
}
