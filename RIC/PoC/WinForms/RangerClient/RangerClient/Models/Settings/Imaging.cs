using FujiXerox.RangerClient.Enums;

namespace FujiXerox.RangerClient.Models.Settings
{
    public class Imaging
    {
        //[Imaging]   - This section that defines general imaging parameters. 
        //
        //RootCaptureDirectory =  - All image files will be created in sub-directories beneath this directory.
        //                          Please note that some transport types such as the Unisys DP500, 1150, etc.
        //                          and NCR 779Xs require an image capture server (ICS) PC.  For these transport
        //                          types, the ICS controls where images are stored and Ranger cannont control
        //                          where the root directory is located. Therefore, the image root capture 
        //                          directory must be defined using the transport vendor's instruction for when
        //                          using these transport types.
        //
        //                          If image fileset storage (see "storagefile" below) is
        //                          defined and this option is not specified, a default
        //                          value of "c:\images" will be used.
        //
        //ImagesNeededToMakePocketDecision= - Set this to indicate if images are needed
        //                                    to make a pocket decision. Valid values:
        //
        //                                    true -      the application requires all
        //                                                requested images to make a
        //                                                pocket decision;
        //                                    false     - the application requires no
        //                                                images to make a pocket
        //                                                decision;
        //                                    FrontOnly - the application requires only
        //                                                the requested front images to
        //                                                make a pocket decision;
        //                                    default   - false.
        //
        //                                    NOTE: Setting this values to "true" may
        //                                          cause some transports to pause or
        //                                          slow while scanning.
        //

        public string RootCaptureDirectory { get; set; }
        public PocketDecision ImagesNeededToMakePocketDecision { get; set; }
    }
}