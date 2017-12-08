using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace FXA.DPSE.Framework.Common.Utils
{
    public static class ConversionUtils
    {
        public static T Deserialize<T>(string json) where T : class
        {
            var result = (T) JsonConvert.DeserializeObject(json, typeof (T),
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All});
            return result;
        }

        public static string Serialize(object model, bool namespaceReference = false)
        {
            var setting = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Include};
            if (namespaceReference) setting.TypeNameHandling = TypeNameHandling.Objects;
            var result = JsonConvert.SerializeObject(model, Formatting.Indented, setting);
            return result;
        }
        /// <summary>
        /// Serialize with WCF DataContractJsonSerializer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            if (null == obj) throw new ArgumentNullException("obj");

            string result;
            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream))
                {
                    result = streamReader.ReadToEnd(); streamReader.Close();
                }
            }

            return result;
        }
    }

}
