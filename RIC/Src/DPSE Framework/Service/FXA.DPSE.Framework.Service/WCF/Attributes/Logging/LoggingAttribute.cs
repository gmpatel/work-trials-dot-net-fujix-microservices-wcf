namespace FXA.DPSE.Framework.Service.WCF.Attributes.Logging
{
    public class LoggingBehaviorAttribute : ServiceBehaviorBase
    {
        public LoggingBehaviorAttribute() : base(new LoggingMessageInspector())
        {
        }
    }
}