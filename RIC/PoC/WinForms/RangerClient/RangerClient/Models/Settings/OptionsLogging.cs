using FujiXerox.RangerClient.Attributes;

namespace FujiXerox.RangerClient.Models.Settings
{
    /// <summary>
    /// If problems are encountered when Ranger reads this GenericOptions.ini file,
    /// then Ranger will write messages to a file called RangerGenericOptions.log 
    /// in the current directory. The log file is deleted and recreated each time 
    /// Ranger reads this GenericOptions.ini file.
    /// </summary>
    [SectionName("OptionsLogging")]
    public class OptionsLogging
    {
        /// <summary>
        ///  Enabled: true or false (default)
        /// </summary>
        public bool Enabled { get; set; }
 
        /// <summary>
        ///  Path=Default (default) or the full path to the preferred location.
        /// </summary>
        public string Path { get; set; }
    }
}