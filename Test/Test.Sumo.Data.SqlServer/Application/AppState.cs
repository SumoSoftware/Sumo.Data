using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.SqlServer.Application
{
    /// <summary>
    /// implementation of the json contained in the secrets file
    /// sample json:
    /// {"ConnectionString":"sql server stuff goes here"}
    /// </summary>
    internal sealed class Secrets
    {
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// contains readonly resources fetched from the secrets file
    /// </summary>
    internal static class AppState
    {
        static AppState()
        {
            var secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText($@".\Application\{nameof(AppState)}.secrets"));
            var secretProperties = typeof(Secrets).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where((p) => p.CanRead == true || p.CanWrite == true);
            var appStateProperties = typeof(AppState).GetProperties(BindingFlags.Public | BindingFlags.Static).Where((p) => p.CanRead == true || p.CanWrite == false);
            foreach (var appStateProperty in appStateProperties)
            {
                var secretProperty = secretProperties.Where((p) => p.Name == appStateProperty.Name).FirstOrDefault();
                if (secretProperty != null) appStateProperty.SetValue(null, secretProperty.GetValue(secrets));
            }
        }

        public static string ConnectionString { get; private set; }
    }
}