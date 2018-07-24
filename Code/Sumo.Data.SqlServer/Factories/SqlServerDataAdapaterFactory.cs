using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sumo.Data.Factories.SqlServer
{
    public sealed class SqlServerDataAdapterFactory : IDataAdapterFactory
    {
        public DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
            if (!(dbCommand is SqlCommand)) throw new ArgumentException($"Type of {nameof(dbCommand)} must be {nameof(SqlCommand)}.");

            return new SqlDataAdapter((SqlCommand)dbCommand);
        }
    }
}
