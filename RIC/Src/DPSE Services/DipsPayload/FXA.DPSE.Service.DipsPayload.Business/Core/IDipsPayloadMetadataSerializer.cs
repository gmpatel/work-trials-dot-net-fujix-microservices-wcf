using System.Xml.Serialization;

namespace FXA.DPSE.Service.DipsPayload.Business.Core
{
    public interface IDipsPayloadMetadataSerializer
    {
        string SerializeWithCustomNamespace<T>(T value, XmlSerializerNamespaces xns = null);
    }
}