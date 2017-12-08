using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Proxy.Audit;
using FXA.DPSE.Framework.Service.WCF.Proxy.Logging;
using FXA.DPSE.Service.DTO.PaymentInstruction;

namespace FXA.DPSE.Service.PaymentInstruction.Decomposer
{
    public class PaymentInstructionActivity <TRequest,TResponse> 
    {
        public string Name { get; set; }
        public IPaymentInstructionHandler<TRequest,TResponse> Handler { get; set; }
    }

    public interface IPaymentInstructionComposite
    {
        List<PaymentInstructionActivity> 
    }

    public interface IPaymentInstructionHandler
    {
    }
    public interface IPaymentInstructionHandler<in TRequest, out TResponse> : IPaymentInstructionHandler
    {
        TResponse Execute(TRequest request, string serviceUrl);
    }

    public class PaymentInstructionWorkflow
    {
        public PaymentInstructionWorkflow(IPaymentInstructionComposite)
        {
            
        }
        public PaymentInstructionResponse Run(PaymentInstructionRequest paymentInstructionRequest)
        {
            
        }
    }
    public class TraceTrackingHandler<TTraceTrackingRequest, TTraceTrackingResponse> : IPaymentInstructionHandler<TTraceTrackingRequest, TTraceTrackingResponse>
    {
        private readonly IAuditProxy _auditProxy;
        private readonly ILoggingProxy _loggingProxy;

        public TraceTrackingHandler(IAuditProxy auditProxy, ILoggingProxy loggingProxy)
        {
            _auditProxy = auditProxy;
            _loggingProxy = loggingProxy;
        }

        public TTraceTrackingResponse Execute(TTraceTrackingRequest request, string serviceUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}
