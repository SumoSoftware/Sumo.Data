using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sumo.Data.Schema
{
    public static class EntityExtensions
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string ToJson(this Entity entity)
        {
            return JsonConvert.SerializeObject(entity, _settings);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] ToBytes(this Entity entity)
        {
            byte[] result = null;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, entity);
                stream.Flush();
                stream.Position = 0;
                result = stream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// user is responsible for disposing stream!
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>MemoryStream - remember to dispose!</returns>
        public static Stream ToStream(this Entity entity)
        {
            var result = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(result, entity);
            result.Flush();
            result.Position = 0;
            return result;
        }

        public static T FromBytes<T>(this byte[] bytes) where T : Entity
        {
            var result = default(T);
            using (var stream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                stream.Position = 0;
                result = (T)formatter.Deserialize(stream);
            }
            return result;
        }

        public static T FromStream<T>(this Stream stream) where T : Entity
        {
            var result = default(T);
            var formatter = new BinaryFormatter();
            stream.Position = 0;
            result = (T)formatter.Deserialize(stream);
            return result;
        }
    }
}
