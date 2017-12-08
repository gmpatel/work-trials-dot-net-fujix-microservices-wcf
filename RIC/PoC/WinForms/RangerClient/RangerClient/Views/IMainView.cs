using System.Collections.Generic;
using System.Data;
using System.Drawing;
using AxRANGERLib;

namespace FujiXerox.RangerClient.Views
{
    public interface IMainView
    {
        AxRanger AxRanger { get; }
        string ScannerStatus { set; }
        string EventStatus { set; }
        bool StartUpEnabled { set; }
        bool ShutDownEnabled { set; }
        bool EnableOptionsEnabled { set; }
        bool StartFeedingEnabled { set; }
        bool StopFeedingEnabled { set; }
        bool PrepareToChangeOptionsEnabled { set; }
        Image FrontImage { set; }
        Image RearImage { set; }
        List<string> DataRow { set; }
    }
}
