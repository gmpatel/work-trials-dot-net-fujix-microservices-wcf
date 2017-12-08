namespace FujiXerox.RangerClient.Models.Settings
{
    /// <summary>
    ///  these sections define additional stamp 
    //                 options that are required by some transports.
    /// </summary>
    public class RearStamp
    {
        /// <summary>
        ///  offset from leading edge in millimeters for stamps 
        ///  that cannot be positioned on a per item basis.                   
        ///  The default value is zero.
        /// 
        /// For rear stamps that support trail edge triggering, a value of -1 
        //  indicates that the stamp should be applied 1.5 inches from the 
        //  trailing edge of the item.
        /// Common StaticPosition values in millimeters:
        ///
        ///  ABA defined endorsement zones for rear stamp
        ///
        ///   1  First transit endorsement 0.05 inches from the leading edge
        ///  38  Second transit endorsement 1.5 inches from the leading edge
        ///  77  Bank of first deposit endorsement 3.0 inches from the leading edge
        ///  -1  Payee endorsement zone 1.5 inches from the trailing edge
        /// </summary>
        public string StaticPosition { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile1 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile2 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile3 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile4 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile5 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile6 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile7 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile8 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile9 { get; set; }
        /// <summary>
        /// StampFileN =   - Where 'N' is a number from 1 to 10.  This is the FULL PATH 
        //                 name of a stamp file to use with transports that support 
        //                 stamp switching while running items.
        //
        //                 Blank entries between file names are not allowed. Ranger 
        //                 will stop reading file names when a blank entry is detected.
        //
        //                 The format of the contents of the stamp files is 
        //                 transport dependent.
        /// </summary>
        public string StampFile10 { get; set; } 
    }
}