using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sumo.Data.Orm.Repositories
{
    public interface IRepository
    {
        T Write<T>(T entity) where T : class;

        Task<T> WriteAsync<T>(T entity) where T : class;

        T Read<T>(object searchKey) where T : class;

        Task<T> ReadAsync<T>(object searchKey) where T : class;

        T[] Read<T>(Dictionary<string, object> parameters) where T : class;

        Task<T[]> ReadAsync<T>(Dictionary<string, object> parameters) where T : class;
    }
}


