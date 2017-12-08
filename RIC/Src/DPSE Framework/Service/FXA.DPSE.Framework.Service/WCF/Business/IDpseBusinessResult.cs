namespace FXA.DPSE.Framework.Service.WCF.Business
{
    public interface IDpseBusinessResult<out T>
    {
        T Result { get; }
    }
}