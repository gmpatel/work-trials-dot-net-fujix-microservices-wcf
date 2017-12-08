namespace FujiXerox.RangerClient.Models.Settings
{
    public class FrontEndorser
    {
        /// <summary>
        ///      Physical: typically inkjet based imprinting on the actual item.
        ///
        ///      Electronic: a.k.a. "virtual" endorsing on the item image only, no change
        ///                  to the actual item.
        ///
        ///      All: every available endorsement type will be applied to each item 
        ///           scanned.  This is the default value.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///      Batch:  endorsing the entire batch the same.
        ///	
        ///      CurrentItem:  endorsing each item as processed
        ///
        ///      NextItem:  endorsing the next item processed
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        ///      If "true", Ranger will emulate specified endorser mode when possible.
        /// </summary>
        public bool EnableEmulationOfEndorserMode { get; set; }

    }
}