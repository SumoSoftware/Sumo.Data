using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Factories
{
    public interface IConnectionFactory
    {
        DbConnection Open(string connectionString);

        Task<DbConnection> OpenAsync(string connectionString);
    }
}
