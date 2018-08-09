using Sumo.Data.Expressions;
using Sumo.Data.Schema;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Sumo.Data.Orm
{
    public class Repository : IRepository
    {
        private readonly IOrmDataComponentFactory _ormDataComponentFactory;

        public Repository(IOrmDataComponentFactory ormDataComponentFactory) : base()
        {
            _ormDataComponentFactory = ormDataComponentFactory ?? throw new ArgumentNullException(nameof(ormDataComponentFactory));
        }

        public T Read<T>(object searchKey, DbTransaction dbTransaction = null) where T : class
        {
            if (searchKey == null) throw new ArgumentNullException(nameof(searchKey));
            if (EntityInfoCache<T>.PrimaryKeyProperties.Length > 1) throw new NotSupportedException($"{TypeInfoCache<T>.FullName} has a multi-part primary key. Use method 'T[] Read<T>(Dictionary<string, object> parameters) where T : class' instead.");

            var parameters = new Dictionary<string, object>();
            var primaryKey = EntityInfoCache<T>.PrimaryKeyProperties[0];
            parameters.Add(primaryKey.Name, searchKey);

            var results = Read<T>(parameters, dbTransaction);

            return results != null && results.Length > 0 ? results[0] : null;
        }

        public async Task<T> ReadAsync<T>(object searchKey, DbTransaction dbTransaction = null) where T : class
        {
            if (searchKey == null) throw new ArgumentNullException(nameof(searchKey));
            if (EntityInfoCache<T>.PrimaryKeyProperties.Length > 1) throw new NotSupportedException($"{TypeInfoCache<T>.FullName} has a multi-part primary key. Use method 'T[] Read<T>(Dictionary<string, object> parameters) where T : class' instead.");

            var parameters = new Dictionary<string, object>();
            var primaryKey = EntityInfoCache<T>.PrimaryKeyProperties[0];

            parameters.Add(primaryKey.Name, searchKey);

            var results = await ReadAsync<T>(parameters, dbTransaction);

            return results != null && results.Length > 0 ? results[0] : null;
        }

        public T[] Read<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null) where T : class
        {
            T[] result = null;
            using (var connection = _ormDataComponentFactory.Open())
            using (var reader = new SqlReader(connection, _ormDataComponentFactory, _ormDataComponentFactory))
            {
                var sql = _ormDataComponentFactory.SqlStatementBuilder.GetSelectStatement<T>(parameters);
                var dataSet = reader.Read(sql, parameters, dbTransaction);
                if (dataSet.Tables.Count > 0) result = dataSet.Tables[0].Rows.ToArray<T>();
            }
            return result;
        }

        public async Task<T[]> ReadAsync<T>(Dictionary<string, object> parameters, DbTransaction dbTransaction = null) where T : class
        {
            T[] result = null;
            using (var connection = await _ormDataComponentFactory.OpenAsync())
            using (var reader = new SqlReader(connection, _ormDataComponentFactory, _ormDataComponentFactory))
            {
                var sql = _ormDataComponentFactory.SqlStatementBuilder.GetSelectStatement<T>(parameters);
                var dataSet = await reader.ReadAsync(sql, parameters, dbTransaction);
                if (dataSet.Tables.Count > 0) result = await dataSet.Tables[0].Rows.ToArrayAsync<T>();
            }
            return result;
        }

        private Dictionary<string, object> GetWriteParameters<T>() where T : class
        {
            var result = new Dictionary<string, object>(EntityInfoCache<T>.NonAutoIncrementProperties.Length);
            for (var i = 0; i < EntityInfoCache<T>.NonAutoIncrementProperties.Length; ++i)
            {
                result[_ormDataComponentFactory.SchemaParameterNames.GetWriteParameterName<T>(i)] = null;
            }
            return result;
        }

        private void SetWriteParameters<T>(T entity, Dictionary<string, object> parameters) where T : class
        {
            for (var i = 0; i < EntityInfoCache<T>.NonAutoIncrementProperties.Length; ++i)
            {
                parameters[_ormDataComponentFactory.SchemaParameterNames.GetWriteParameterName<T>(i)] =
                    EntityInfoCache<T>.NonAutoIncrementProperties[i].GetValue(entity) ?? DBNull.Value;
            }
        }

        public void Write<T>(T entity, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class
        {
            var tableExistsSql = _ormDataComponentFactory.SqlStatementBuilder.GetExistsStatement<T>();
            using (var connection = _ormDataComponentFactory.Open())
            using (var command = new Command(connection, _ormDataComponentFactory))
            {
                var transaction = dbTransaction ?? _ormDataComponentFactory.BeginTransaction(connection);
                try
                {
                    var tableExists = command.ExecuteScalar<bool>(tableExistsSql, transaction);
                    if (!tableExists)
                    {
                        if (autoCreateTable)
                        {
                            var tableDefinition = _ormDataComponentFactory.ScriptBuilder.BuildTable<T>();
                            var createTableSql = _ormDataComponentFactory.ScriptBuilder.BuildCreateScript(tableDefinition);
                            command.Execute(createTableSql, transaction);
                        }
                        else
                        {
                            throw new TableNotFoundException(TypeInfoCache<T>.Name);
                        }
                    }
                    var insertSql = _ormDataComponentFactory.SqlStatementBuilder.GetInsertStatement<T>();
                    var parameters = GetWriteParameters<T>();
                    SetWriteParameters(entity, parameters);
                    var rowsInserted = command.Execute(insertSql, parameters, transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    if (dbTransaction == null) transaction.Dispose();
                    connection.Close();
                }
            }
        }

        public async Task WriteAsync<T>(T entity, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class
        {
            await Task.Run(async () =>
            {
                var tableExistsSql = _ormDataComponentFactory.SqlStatementBuilder.GetExistsStatement<T>();
                using (var connection = _ormDataComponentFactory.Open())
                using (var command = new Command(connection, _ormDataComponentFactory))
                {
                    var transaction = dbTransaction ?? _ormDataComponentFactory.BeginTransaction(connection);
                    try
                    {
                        var tableExists = await command.ExecuteScalarAsync<bool>(tableExistsSql, transaction);
                        if (!tableExists)
                        {
                            if (autoCreateTable)
                            {
                                var tableDefinition = _ormDataComponentFactory.ScriptBuilder.BuildTable<T>();
                                var createTableSql = _ormDataComponentFactory.ScriptBuilder.BuildCreateScript(tableDefinition);
                                await command.ExecuteAsync(createTableSql, transaction);
                            }
                            else
                            {
                                throw new TableNotFoundException(TypeInfoCache<T>.Name);
                            }
                        }
                        var insertSql = _ormDataComponentFactory.SqlStatementBuilder.GetInsertStatement<T>();
                        var parameters = GetWriteParameters<T>();
                        SetWriteParameters(entity, parameters);
                        var rowsInserted = await command.ExecuteAsync(insertSql, parameters, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (dbTransaction == null) transaction.Dispose();
                        connection.Close();
                    }
                }
            });
        }

        public void Write<T>(T[] entities, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class
        {
            var tableExistsSql = _ormDataComponentFactory.SqlStatementBuilder.GetExistsStatement<T>();
            using (var connection = _ormDataComponentFactory.Open())
            using (var command = new Command(connection, _ormDataComponentFactory))
            {
                var transaction = dbTransaction ?? _ormDataComponentFactory.BeginTransaction(connection);
                try
                {
                    var tableExists = command.ExecuteScalar<bool>(tableExistsSql, transaction);
                    if (!tableExists)
                    {
                        if (autoCreateTable)
                        {
                            var tableDefinition = _ormDataComponentFactory.ScriptBuilder.BuildTable<T>();
                            var createTableSql = _ormDataComponentFactory.ScriptBuilder.BuildCreateScript(tableDefinition);
                            command.Execute(createTableSql, transaction);
                        }
                        else
                        {
                            throw new TableNotFoundException(TypeInfoCache<T>.Name);
                        }
                    }
                    var insertSql = _ormDataComponentFactory.SqlStatementBuilder.GetInsertStatement<T>();
                    var parameters = GetWriteParameters<T>();
                    for (var i = 0; i < entities.Length; ++i)
                    {
                        var entity = entities[i];
                        SetWriteParameters(entity, parameters);
                        var rowsInserted = command.Execute(insertSql, parameters, transaction);
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    if (dbTransaction == null) transaction.Dispose();
                    connection.Close();
                }
            }
        }

        public async Task WriteAsync<T>(T[] entities, DbTransaction dbTransaction = null, bool autoCreateTable = true) where T : class
        {
            await Task.Run(async () =>
            {
                var tableExistsSql = _ormDataComponentFactory.SqlStatementBuilder.GetExistsStatement<T>();
                using (var connection = _ormDataComponentFactory.Open())
                using (var command = new Command(connection, _ormDataComponentFactory))
                {
                    var transaction = dbTransaction ?? _ormDataComponentFactory.BeginTransaction(connection);
                    try
                    {
                        var tableExists = await command.ExecuteScalarAsync<bool>(tableExistsSql, transaction);
                        if (!tableExists)
                        {
                            if (autoCreateTable)
                            {
                                var tableDefinition = _ormDataComponentFactory.ScriptBuilder.BuildTable<T>();
                                var createTableSql = _ormDataComponentFactory.ScriptBuilder.BuildCreateScript(tableDefinition);
                                await command.ExecuteAsync(createTableSql, transaction);
                            }
                            else
                            {
                                throw new TableNotFoundException(TypeInfoCache<T>.Name);
                            }
                        }
                        var insertSql = _ormDataComponentFactory.SqlStatementBuilder.GetInsertStatement<T>();
                        var parameters = GetWriteParameters<T>();
                        Parallel.For(0, entities.Length - 1, async i =>
                        {
                            var entity = entities[i];
                            SetWriteParameters(entity, parameters);
                            var rowsInserted = await command.ExecuteAsync(insertSql, parameters, transaction);
                        });
                        //for (var i = 0; i < entities.Length; ++i)
                        //{
                        //    var entity = entities[i];
                        //    SetWriteParameters(entity, parameters);
                        //    var rowsInserted = await command.ExecuteAsync(insertSql, parameters, transaction);
                        //}
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (dbTransaction == null) transaction.Dispose();
                        connection.Close();
                    }
                }
            });
        }

        public T[] Read<T>(IExpression expression, DbTransaction dbTransaction = null)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> ReadAsync<T>(IExpression expression, DbTransaction dbTransaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
