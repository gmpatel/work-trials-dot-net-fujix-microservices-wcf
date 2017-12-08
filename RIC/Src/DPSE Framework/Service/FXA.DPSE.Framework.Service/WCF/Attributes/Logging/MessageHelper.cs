using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FXA.DPSE.Framework.Service.WCF.Attributes.Logging
{
    public static class MessageHelper
    {
        public static string MessageToString(this Message channelMessage, ref Message message)
        {
            var messageFormat = GetMessageContentFormat(message);
            var memoryStream = new MemoryStream();
            XmlDictionaryWriter writer = null;
            switch (messageFormat)
            {
                case WebContentFormat.Default:
                case WebContentFormat.Xml:
                    writer = XmlDictionaryWriter.CreateTextWriter(memoryStream);
                    break;
                case WebContentFormat.Json:
                    writer = JsonReaderWriterFactory.CreateJsonWriter(memoryStream);
                    break;
                case WebContentFormat.Raw:
                    return ReadRawBody(ref message);
            }

            message.WriteMessage(writer);
            writer.Flush();
            var messageBody = Encoding.UTF8.GetString(memoryStream.ToArray());
            memoryStream.Position = 0;
            var reader = messageFormat == WebContentFormat.Json
                ? JsonReaderWriterFactory.CreateJsonReader(memoryStream, XmlDictionaryReaderQuotas.Max)
                : XmlDictionaryReader.CreateTextReader(memoryStream, XmlDictionaryReaderQuotas.Max);

            var newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        private static string ReadRawBody(ref Message message)
        {
            var bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            var bodyBytes = bodyReader.ReadContentAsBase64();
            var messageBody = Encoding.UTF8.GetString(bodyBytes);

            // Now to recreate the message
            var ms = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            writer.WriteStartElement("Binary");
            writer.WriteBase64(bodyBytes, 0, bodyBytes.Length);
            writer.WriteEndElement();
            writer.Flush();
            ms.Position = 0;
            var reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            var newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        private static WebContentFormat GetMessageContentFormat(Message message)
        {
            var format = WebContentFormat.Default;
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                var bodyFormat = (WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name];
                format = bodyFormat.Format;
            }

            return format;
        }
    }
}
