using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Orm.Repositories
{
    public interface IRepository
    {
        void Write<T>(T entity, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class;
        Task WriteAsync<T>(T entity, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class;

        void Write<T>(T[] entities, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class;
        Task WriteAsync<T>(T[] entities, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class;

        T Read<T>(object searchKey, DbTransaction dbTransaction = null) where T : class;
        Task<T> ReadAsync<T>(object searchKey, DbTransaction dbTransaction = null) where T : class;

        T[] Read<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null) where T : class;
        Task<T[]> ReadAsync<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null) where T : class;

        //todo: add read methods that support Sumo.Data.Expressions
    }
}


