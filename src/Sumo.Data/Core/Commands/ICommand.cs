using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data
{
    public interface ICommand : IDisposable
    {
        bool Prepare(string sql, Dictionary<string, object> queryParams = null);
        void SetParameterValues(string sql, Dictionary<string, object> queryParams = null);

        int Execute(string sql, DbTransaction dbTransaction = null);
        Task<int> ExecuteAsync(string sql, DbTransaction dbTransaction = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="dbTransaction"></param>
        /// <returns>number of rows affected</returns>
        int Execute(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="dbTransaction"></param>
        /// <returns>number of rows affected</returns>
        Task<int> ExecuteAsync(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null);

        T ExecuteScalar<T>(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null);
        Task<T> ExecuteScalarAsync<T>(string sql, Dictionary<string, object> parameters, DbTransaction dbTransaction = null);

        T ExecuteScalar<T>(string sql, DbTransaction dbTransaction = null);
        Task<T> ExecuteScalarAsync<T>(string sql, DbTransaction dbTransaction = null);
    }
}
