using FujiXerox.RangerClient.Attributes;

namespace FujiXerox.RangerClient.Models.Settings
{
    /// <summary>
    ///  Section to enable devices that impact performance on some transports.
    ///  If this section is not present or if an entry is not present, then
    ///  the missing device(s) will be disabled.
    /// </summary>
    [SectionName("OptionalDevices")]
    public class OptionalDevices
    {
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedMicrEncoder { get; set; }
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedFrontEndorser { get; set; }
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedRearEndorser { get; set; }
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedFrontStamp { get; set; }
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedRearStamp { get; set; }
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedMicrofilmer { get; set; }
        /// <summary>
        /// Legal values: "true", any other value implies "false"
        /// </summary>
        public bool NeedDoubleDocDetection { get; set; }

        /// <summary>
        ///  If "true", OCR reading is required.
        /// </summary>
        public bool NeedOcr  { get; set; }

        public bool NeedOcrReader1  { get; set; }
        /// <summary>
        ///  If "true", then NeedOcrReader1 must also be true
        /// </summary>
        public bool NeedOcrReader2  { get; set; }
        /// <summary>
        ///  If "true", then NeedOcrReader1 and NeedOcrReader2 must also be true
        /// </summary>
        public bool NeedOcrReader3 { get; set; }
        
        /// <summary>
        ///  If "true", imaging is required.
        /// </summary>
        public bool NeedImaging { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedFrontImage1 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedRearImage1 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedFrontImage2 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedRearImage2 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedFrontImage3 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedRearImage3 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedFrontImage4 { get; set; }
        /// <summary>
        ///  Is set to "true" then the image will be scanned, if possible
        /// </summary>
        public bool NeedRearImage4 { get; set; }
        /// <summary>
        /// if "X9.100-181ToughTiff",    (default)                  
        /// then B&W images will be converted to conform to the X9B Tough-Tiff specification
        /// if "ScannerRaw", images will not be converted
        /// </summary>
        [ValueName("BWTiffCompliance")]
        public string BwTiffCompliance { get; set; }
        /// <summary>
        ///  If "true", image quality analysis is requested.
        /// </summary>
        [ValueName("NeedIQA")]
        public bool NeedIqa { get; set; }
        /// <summary>
        ///  If "true", IQA results needed in SetItemOutput event.
        /// </summary>
        [ValueName("NeedIQAUpstream")]
        public bool NeedIqaUpstream { get; set; }
        /// <summary>
        ///  If "true", IQA results available in ItemInPocket event.
        /// </summary>
        [ValueName("NeedIQADownstream")]
        public bool NeedIqaDownstream { get; set; }

        public bool NeedPeacemaker { get; set; }
        public bool NeedPeacemakerUpstream { get; set; }
        public bool NeedPeacemakerDownstream { get; set; }
        /// <summary>
        ///  If "true", then pocket groups will be read from the [Pockets] section.
        /// </summary>
        public bool NeedPocketGroups { get; set; }
        /// <summary>
        ///  If "true", enable remote monitoring, if Scout is installed.
        /// </summary>
        public bool NeedScout { get; set; }

    }
}