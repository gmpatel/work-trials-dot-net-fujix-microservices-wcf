using FujiXerox.RangerClient.Attributes;

namespace FujiXerox.RangerClient.Models.Settings
{
    [SectionName("IQA.General")]
    public class IqaGeneral
    {
        [ValueName("EnableIQA")]
        public bool EnableIqa { get; set; } 
    }
}