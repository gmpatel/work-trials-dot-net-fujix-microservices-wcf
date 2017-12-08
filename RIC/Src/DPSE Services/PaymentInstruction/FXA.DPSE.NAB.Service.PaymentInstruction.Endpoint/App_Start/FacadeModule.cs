using Autofac;
using FXA.DPSE.Service.DTO.DipsPayload;
using FXA.DPSE.Service.DTO.PaymentValidation;
using FXA.DPSE.Service.DTO.ShadowPost;
using FXA.DPSE.Service.DTO.TraceTracking;
using FXA.DPSE.Service.PaymentInstruction;
using FXA.DPSE.Service.PaymentInstruction.Business.Entities;
using FXA.DPSE.Service.PaymentInstruction.Core;
using FXA.DPSE.Service.PaymentInstruction.Facade;
using FXA.DPSE.Service.PaymentInstruction.Facade.Core;

namespace FXA.DPSE.NAB.Service.PaymentInstruction.Endpoint
{
    public class FacadeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PaymentInstructionWorkflow>().As<IPaymentInstructionWorkflow>();

            builder.RegisterType<TraceTrackingFacade>().As<IPaymentInstructionFacade<TrackingIdentifierResult, ElectronicTraceTrackingResponse>>();
            builder.RegisterType<DipsPayloadFacade>().As<IPaymentInstructionFacade<TrackingIdentifierResult, DipsPayloadBatchResponse>>();
            builder.RegisterType<PaymentValidationFacade>().As<IPaymentInstructionFacade<TrackingIdentifierResult, PaymentValidationResponse>>();
            builder.RegisterType<ShadowPostFacade>().As<IPaymentInstructionFacade<TrackingIdentifierResult, ShadowPostResponse>>();
        }
    }
}