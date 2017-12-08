using FujiXerox.RangerClient.Enums;

namespace FujiXerox.RangerClient.Models.Settings
{
    public class RearImage2
    {
        //[FrontImageN]   - where 'N' is the image number defined in the transport information file. 
        //                  This section is only read if the corresponding NeedFrontImageN = true.
        //
        //StorageFile=    - Extention of the file name in which this image will be stored.
        //                  Legal values for [FrontImage1]: "FIM" or "None"
        //                                   [FrontImage2]: "FI2" or "None"
        //                  "None" implies that Ranger should scan the image but not store it in a file.
        //

        public RearImage StorageFile { get; set; }
    }
}