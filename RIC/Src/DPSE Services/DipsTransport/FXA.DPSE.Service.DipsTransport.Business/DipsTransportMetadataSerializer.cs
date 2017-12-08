using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FXA.DPSE.Service.DipsTransport.Business.Core;

namespace FXA.DPSE.Service.DipsTransport.Business
{
    public class DipsTransportMetadataSerializer : IDipsTransportMetadataSerializer
    {
        public string SerializeWithCustomNamespace<T>(T value, XmlSerializerNamespaces xns = null)
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                if (xns == null)
                {
                    xns = new XmlSerializerNamespaces();
                    xns.Add("ns", "http://lombard.aus.fujixerox.com/outclearings/DPSEEndOfDay");
                }

                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new Utf8StringWriter();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlserializer.Serialize(writer, value, xns);
                    return stringWriter.ToString().Replace("_x003A_", ":");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error occurred - {0}", ex.Message), ex);
            }
        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }
}