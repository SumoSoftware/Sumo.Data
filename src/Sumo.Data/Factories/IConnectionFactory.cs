using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Factories
{
    public interface IConnectionFactory
    {
        DbConnection Open(string connectionString);

        DbConnection Open();

        Task<DbConnection> OpenAsync(string connectionString);

        Task<DbConnection> OpenAsync();

        IParameterFactory ParameterFactory { get; }

        IDataAdapterFactory DataAdapterFactory { get; }
    }
}
