using Microsoft.Data.Sqlite;
using Sumo.Data.Sqlite;
using System;
using System.Data.Common;

namespace Sumo.Data.Factories.Sqlite
{
    public class SqliteDataAdapterFactory : IDataAdapterFactory
    {
        public DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
            if (!(dbCommand is SqliteCommand)) throw new ArgumentException($"Type of {nameof(dbCommand)} must be {nameof(SqliteCommand)}.");

            return new SqliteDataAdapter((SqliteCommand)dbCommand);
        }
    }
}
