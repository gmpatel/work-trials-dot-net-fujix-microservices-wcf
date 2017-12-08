using System.Xml.Serialization;

namespace FXA.DPSE.Service.DipsTransport.Business.Core
{
    public interface IDipsTransportMetadataSerializer
    {
        string SerializeWithCustomNamespace<T>(T value, XmlSerializerNamespaces xns = null);
    }
}