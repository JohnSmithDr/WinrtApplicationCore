using System.IO;
using System.Runtime.Serialization.Json;

namespace JohnSmithDr.ApplicationCore.Data
{
    public static class JsonConverter
    {
        public static string Serialize<T>(T value)
        {
            string result = null;
            var settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };
            var api = new DataContractJsonSerializer(value.GetType(), settings);
            using (var stream = new MemoryStream())
            {
                api.WriteObject(stream, value);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static T Deserialize<T>(string text)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var api = new DataContractJsonSerializer(typeof(T));
                var obj = api.ReadObject(stream);
                return (T)obj;
            }
        }
    }
}
