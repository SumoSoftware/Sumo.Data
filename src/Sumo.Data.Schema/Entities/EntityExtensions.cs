using System;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

namespace Sumo.Data.Schema
{
    public static class EntityExtensions
    {
        #region to/from json
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

        public static T ToEntity<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion

        #region to/from bytes
        public static byte[] ToBytes(this Entity entity)
        {
            byte[] result = null;
            using (var stream = entity.ToStream())
            {
                result = stream.ToArray();
            }
            return result;
        }

        public static T ToEntity<T>(this byte[] bytes) where T : Entity
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
        #endregion

        #region to/from streams
        /// <summary>
        /// user provides stream
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="stream"></param>
        /// <returns>MemoryStream - remember to dispose!</returns>
        public static void ToStream(this Entity entity, Stream stream)
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
            entity.ToStream(result);
            return result;
        }

        public static T ToEntity<T>(this Stream stream) where T : Entity
        {
            var result = default(T);
            stream.Position = 0;
            var formatter = new BinaryFormatter();
            result = (T)formatter.Deserialize(stream);
            return result;
        }

        public static void Write(this Stream stream, Entity entity)
        {
            entity.ToStream(stream);
        }

        public static T ReadFromStream<T>(this Stream stream) where T : Entity
        {
            return stream.ToEntity<T>();
        }

        #endregion

        #region to/from compressed streams
        /// <summary>
        /// user is responsible for disposing stream!
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>MemoryStream - remember to dispose!</returns>
        public static MemoryStream ToCompressedStream(this Entity entity)
        {
            var result = new MemoryStream();
            entity.ToCompressedStream(result);
            return result;
        }

        public static T ToEntityFromCompressedStream<T>(this Stream stream) where T : Entity
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

        public static void ToCompressedStream(this Entity entity, Stream stream)
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
        #endregion
    }
}
