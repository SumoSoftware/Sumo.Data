using Sumo.Data.Orm.Factories;
using Sumo.Data.Orm.Types;
using Sumo.Data.Queries;
using Sumo.Data.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sumo.Data.Orm.Repositories
{
    public class Repository : IRepository
    {
        private readonly IFactorySet _factorySet;
        private readonly string _connectionString;

        public Repository(IFactorySet factorySet, string connectionString)
        {
            _factorySet = factorySet ?? throw new ArgumentNullException(nameof(factorySet));
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
        }

        public T Read<T>(object searchKey) where T : class
        {
            if (searchKey == null) throw new ArgumentNullException(nameof(searchKey));
            if (EntityInfoCache<T>.PrimaryKeyProperties.Length > 1) throw new NotSupportedException($"{TypeInfoCache<T>.FullName} has a multi-part primary key. Use method 'T[] Read<T>(Dictionary<string, object> parameters) where T : class' instead.");
            var parameters = new Dictionary<string, object>();
            var primaryKey = EntityInfoCache<T>.PrimaryKeyProperties[0];
            parameters.Add(primaryKey.Name, searchKey);
            var results = Read<T>(parameters);
            return results != null && results.Length > 0 ? results[0] : null;
        }

        public async Task<T> ReadAsync<T>(object searchKey) where T : class
        {
            if (searchKey == null) throw new ArgumentNullException(nameof(searchKey));
            if (EntityInfoCache<T>.PrimaryKeyProperties.Length > 1) throw new NotSupportedException($"{TypeInfoCache<T>.FullName} has a multi-part primary key. Use method 'T[] Read<T>(Dictionary<string, object> parameters) where T : class' instead.");
            var parameters = new Dictionary<string, object>();
            var primaryKey = EntityInfoCache<T>.PrimaryKeyProperties[0];
            parameters.Add(primaryKey.Name, searchKey);
            var results = await ReadAsync<T>(parameters);
            return results != null && results.Length > 0 ? results[0] : null;
        }

        public T[] Read<T>(Dictionary<string, object> parameters) where T : class
        {
            T[] result = null;
            using (var connection = _factorySet.ConnectionFactory.Open(_connectionString))
            using (var reader = new SqlReader(connection, _factorySet.ParameterFactory, _factorySet.DataAdapterFactory))
            {
                var sql = _factorySet.SqlStatementBuilder.GetSelectStatement<T>(parameters);
                var dataSet = reader.Read(sql, parameters);
                if (dataSet.Tables.Count > 0) result = dataSet.Tables[0].Rows.ToArray<T>();
            }
            return result;
        }

        public async Task<T[]> ReadAsync<T>(Dictionary<string, object> parameters) where T : class
        {
            T[] result = null;
            using (var connection = await _factorySet.ConnectionFactory.OpenAsync(_connectionString))
            using (var reader = new SqlReader(connection, _factorySet.ParameterFactory, _factorySet.DataAdapterFactory))
            {
                var sql = _factorySet.SqlStatementBuilder.GetSelectStatement<T>(parameters);
                var dataSet = await reader.ReadAsync(sql, parameters);
                if (dataSet.Tables.Count > 0) result = await dataSet.Tables[0].Rows.ToArrayAsync<T>();
            }
            return result;
        }

        public T Write<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> WriteAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
