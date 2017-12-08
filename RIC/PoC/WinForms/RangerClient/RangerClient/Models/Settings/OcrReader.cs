namespace FujiXerox.RangerClient.Models.Settings
{
    public class OcrReader
    {
        //[OcrReaderN]      - where 'N' is the OCR line number defined in the transport information file. 
        //                    This section is only read if the corresponding NeedOcrReaderN = true.
        //
        //Font=             - Font of OCR line. 
        //                    Legal values: See TransportInfo.ini [OcrReader*] sections.
        //
        //VerticalCenter=   - Vertical center of the OCR line in millimeters from bottom of item.
        //Right=            - Right edge of OCR line in millimeters from the right edge of the item.
        //Width=            - Width of OCR line in millimeters from the "Right" value.
        //Height=           - Height of OCR line in millimeters.
        //------------------------------------------------------------------------------

        //Example:
        //
        //[OcrReader1]
        //Font=E13B
        //VerticalCenter=7
        //Right=0
        //Width=150
        //Height=10

        public string Font { get; set; }

        public string VerticalCenter { get; set; }

        public string Right { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }
    }
}