using Newtonsoft.Json;

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
    }
}
