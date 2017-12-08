namespace FXA.DPSET.Framework.Service.Infrastructure.Attributes.Errors
{
    public class ErrorBehavior : ErrorBehaviorBase
    {
        public ErrorBehavior() : base(new ErrorHandler())
        {
        }
    }
}