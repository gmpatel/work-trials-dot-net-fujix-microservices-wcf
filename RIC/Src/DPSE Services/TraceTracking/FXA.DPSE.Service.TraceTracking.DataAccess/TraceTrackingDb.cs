namespace FXA.DPSE.Service.TraceTracking.DataAccess
{
    public partial class TraceTrackingDb
    {
        private bool _disposed;
        public bool IsDisposed
        {
            get { return _disposed; }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            try
            {
                if (disposing)
                {
                    //dispose of managed resources (if and when we have any)
                }
                //dispose of unmanaged resources (if and when we have any)
                _disposed = true;
            }
            finally
            {
                //dispose on base class
                base.Dispose(disposing);
            }
        }
    }
}