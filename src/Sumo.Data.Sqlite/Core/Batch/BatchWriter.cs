using Microsoft.Data.Sqlite;
using Sumo.Data.Schema;
using Sumo.Data.Schema.Sqlite;
using System;
using System.Data.Common;

namespace Sumo.Data.Sqlite
{
    public class BatchWriter : IBatchWriter
    {
        DbCommand _command;
        DbConnection _connection;

        public void Init(Table table, DbConnection connection)
        {
            var bldr = new SqliteScriptBuilder();
            var cmd = connection.CreateCommand();
            cmd.CommandText = bldr.BuildCreateScript(table);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            
            _connection = connection;
            _command = connection.CreateCommand();

            var paramFactory = new SqliteParameterFactory();

            var fields = String.Empty;
            var values = String.Empty;
            foreach(var param in table.Columns)
            {
                var paramName = $"@{param.Name}";

                fields += String.IsNullOrEmpty(fields) ? $"{param.Name}" : $", {param.Name}";
                values += String.IsNullOrEmpty(values) ? $"{paramName}" : $", {paramName}";

                _command.Parameters.Add(new SqliteParameter($"{paramName}", null));
            }

            _command.CommandText = $"insert into [{table.Name}] ({fields}) values ({values})";
        }

        public void Begin()
        {
            _connection.Open();
            _command.Prepare();
            _command.Transaction = _connection.BeginTransaction();
        }

        public void Execute(object[] items)
        {
            var idx = 0;
            foreach (var obj in items)
            {
                var param = _command.Parameters[idx++];
                param.Value = obj ?? System.DBNull.Value;
            }

            _command.ExecuteNonQuery();
        }

        public void End()
        {
            _command.Transaction.Commit();
            _command.Transaction.Dispose();
            _command.Transaction = null;

            _connection.Dispose();
            _connection = null;

            _command.Dispose();
            _command = null;
        }
    }
}
