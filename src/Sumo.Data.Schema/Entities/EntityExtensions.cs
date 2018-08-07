using System;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

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
            using (var stream = (MemoryStream)entity.ToStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, entity);
                stream.Flush();
                stream.Position = 0;
                result = stream.ToArray();
            }
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

        /// <summary>
        /// user provides stream
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="stream"></param>
        /// <returns>MemoryStream - remember to dispose!</returns>
        public static void WriteToStream(this Entity entity, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, entity);
            stream.Flush();
            stream.Position = 0;
        }

        /// <summary>
        /// user is responsible for disposing stream!
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>MemoryStream - remember to dispose!</returns>
        public static MemoryStream ToStream(this Entity entity)
        {
            var result = new MemoryStream();
            entity.WriteToStream(result);
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

        /// <summary>
        /// user is responsible for disposing stream!
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>MemoryStream - remember to dispose!</returns>
        public static MemoryStream ToCompressedStream(this Entity entity)
        {
            var result = new MemoryStream();

            using (var gzipStream = new GZipStream(result, CompressionMode.Compress, true))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(gzipStream, entity);
                gzipStream.Flush();
            }
            result.Position = 0;

            return result;
        }

        public static void Write(this  Stream stream, Entity entity)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, entity);
        }

        public static T ReadFromStream<T>(this Stream stream) where T : Entity
        {
            var formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(stream);
        }

        public static T FromBytes<T>(this byte[] bytes) where T : Entity
        {
            var result = default(T);

            using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                stream.Position = 0;
                var formatter = new BinaryFormatter();
                result = (T)formatter.Deserialize(gzipStream);
            }

            return result;
        }

        public static void WriteToCompressedStream(this Entity entity, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var gzipStream = new GZipStream(stream, CompressionMode.Compress, true))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(gzipStream, entity);
                gzipStream.Flush();
            }
            stream.Position = 0;
        }
    }
}
