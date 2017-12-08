namespace FujiXerox.RangerClient.Models.Settings
{
    public class PhysicalPocket
    {
        private int Physical { get; set; }
        private string Logical { get; set; }

        public PhysicalPocket(int physical, string logical)
        {
            Physical = physical;
            Logical = logical;
        }

        public override string ToString()
        {
            return string.Format("{0},\"{1}\"", Physical, Logical);
        }
    }
}