using FujiXerox.RangerClient.Enums;

namespace FujiXerox.RangerClient.Models.Settings
{
    public class Pockets
    {
        //[Pockets]          - This section allows you to override the 1-to-1 mapping of 
        //                     physical to logical pockets.  This section is only read if
        //                     NeedPocketGroups=true and the scanner supports this feature.
        //
        //PocketFullCount=   - A pocket is logically full when this many items are sent to the pocket.
        //                     If the full pocket is part of a pocket group, then new items will be sent
        //                     to the next pocket in the group.  When the last pocket in the group is
        //                     full, the operator will be prompted to clear the group.
        //                     If this entry is not present or the value is zero, then the transport's 
        //                     default count will be used.
        //
        //RangerRejectPocket= This is the reject pocket that Ranger sendx items to during exeception handling.
        //                    Many transports send rejects to a fixed pocket.  Ranger will send reject items 
        //                    to this pocket by default if possible. Otherwise Ranger sends rejects to the
        //                    first pocket.  Supported values:
        //                    FirstPocket - Ranger rejects will be sent to the first logical pocket
        //                    LastPocket - Ranger rejects will be sent to the last logical pocket
        //                    UseHardwareDefault - If supported by the hardware, Ranger rejects will be sent 
        //                                        to pocket that the transport normally sends rejects to.
        //                                        This is the default value.
        //
        //PhysicalPocketN=   - Where 'N' is the physical pocket number.
        //                     Value is: logical pocket number, "pocket name"
        //                     Logical pocket number is the one given to Ranger as the destination pocket.
        //                     "pocket name" a two character string displayed to the operator during jam handling.
        //                     The pocket name is optional and if not present, the physical pocket number will
        //                     be displayed to the operator.

        public int PocketFullCount { get; set; }

        public RangerRejectPocket RangerRejectPocket { get; set; }

        public PhysicalPocket PhysicalPocket1 { get; set; }
        public PhysicalPocket PhysicalPocket2 { get; set; }
        public PhysicalPocket PhysicalPocket3 { get; set; }
        public PhysicalPocket PhysicalPocket4 { get; set; }
        public PhysicalPocket PhysicalPocket5 { get; set; }
        public PhysicalPocket PhysicalPocket6 { get; set; }
        public PhysicalPocket PhysicalPocket7 { get; set; }
        public PhysicalPocket PhysicalPocket8 { get; set; }
        public PhysicalPocket PhysicalPocket9 { get; set; }
        public PhysicalPocket PhysicalPocket10 { get; set; }
        public PhysicalPocket PhysicalPocket11 { get; set; }
        public PhysicalPocket PhysicalPocket12 { get; set; }
    }
}