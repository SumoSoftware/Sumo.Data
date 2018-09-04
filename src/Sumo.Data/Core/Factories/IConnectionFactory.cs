using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface IConnectionFactory
    {
        DbConnection Open();
        DbConnection Open(string connectionString);
        DbConnection Open(IConnectionStringFactory connectionStringFactory);
        Task<DbConnection> OpenAsync();
        Task<DbConnection> OpenAsync(string connectionString);
        Task<DbConnection> OpenAsync(IConnectionStringFactory connectionStringFactory);
    }
}
