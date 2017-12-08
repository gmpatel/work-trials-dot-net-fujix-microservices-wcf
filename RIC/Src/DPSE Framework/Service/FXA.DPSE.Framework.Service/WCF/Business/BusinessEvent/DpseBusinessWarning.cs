namespace FXA.DPSE.Framework.Service.WCF.Business.BusinessEvent
{
    public class DpseBusinessWarning : IDpseBusinessEvent
    {
        public DpseBusinessWarning() { }
        public DpseBusinessWarning(DpseBusinessWarningType warningType, string message)
        {
            Type = warningType;
            Message = message;
        }

        public DpseBusinessWarningType Type { get; private set; }
        public string Message { get; private set; }
    }
}
