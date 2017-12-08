using System.Drawing;

namespace FujiXerox.RangerClient.Models
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        public string MicrText { get; set; }
        public string OcrText { get; set; }
        public Image FrontImage { get; set; }
        public Image RearImage { get; set; }
    }
}
