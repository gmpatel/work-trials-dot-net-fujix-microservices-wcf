namespace FXA.DPSE.Framework.Service.WCF.Business
{
    public abstract class DpseBusinessResultBase<T> : IDpseBusinessResult<T> where T : new()
    {
        private readonly T _resultInstance = new T();
        public T Result
        {
            get { return _resultInstance; }
        }
    }
}
