using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface IConnectionStringFactory
    {
        string GetConnectionString();
        Task<string> GetConnectionStringAsync();
    }
}
