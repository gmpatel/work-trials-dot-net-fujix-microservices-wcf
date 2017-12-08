using Autofac;
using FXA.DPSE.Service.DipsPayload.Business;
using FXA.DPSE.Service.DipsPayload.Business.Core;

namespace FXA.DPSE.NAB.Service.DipsPayload.Endpoint
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<PayloadBatchCreator>().As<IPayloadBatchCreator>();
            builder.RegisterType<DipsPayloadMetadataCreator>().As<IDipsPayloadMetadataCreator>();
            builder.RegisterType<DipsPayloadImagesCreator>().As<IDipsPayloadImagesCreator>();
            builder.RegisterType<DipsPayloadMetadataSerializer>().As<IDipsPayloadMetadataSerializer>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
        }
    }
}