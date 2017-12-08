using AxRANGERLib;

namespace FujiXerox.RangerClient.Views
{
    public interface ISettingsView
    {
        AxRanger AxRanger { get; set; }
        string GeneralOptionFilename { get; set; }
        string DriverOptionFilename { get; set; }
    }
}