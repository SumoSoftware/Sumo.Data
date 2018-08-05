using System.Text;

namespace Sumo.Data.Sqlite
{
    public static class StringExtensions
    {
        public static string ToUTF8(this string value, Encoding encoding)
        {
            var bytes = encoding.GetBytes(value);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ToUTF8(this string value)
        {
            return value.ToUTF8(Encoding.Default);
        }
    }
}
