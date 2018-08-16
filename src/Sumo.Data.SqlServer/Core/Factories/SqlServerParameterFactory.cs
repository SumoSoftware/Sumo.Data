using Sumo.Data.Schema.SqlServer;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Sumo.Data.SqlServer
{
    public class SqlServerParameterFactory : IParameterFactory
    {
        public DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqlParameter(name, value)
            {
                Direction = direction,
                SqlDbType = value.GetType().ToSqlDbType()
            };
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqlParameter(name, value)
            {
                Direction = direction,
                Size = size,
                SqlDbType = value.GetType().ToSqlDbType()
            };
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqlParameter(name, type.ToSqlDbType(), size)
            {
                Direction = direction
            };
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqlParameter(name, type.ToSqlDbType())
            {
                Direction = direction
            };
        }

        public DbParameter CreateParameter(string name, DbType type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqlParameter(name, type.ToSqlDbType());
        }

        public DbParameter CreateParameter(string name, PropertyInfo property)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqlParameter(name, property.PropertyType.ToSqlDbType());
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqlParameter(name, property.PropertyType.ToSqlDbType())
            {
                Direction = direction
            };
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqlParameter(name, property.PropertyType.ToSqlDbType(), size)
            {
                Direction = direction
            };
        }

        public DbParameter CreateReturnParameter(string name)
        {
            return new SqlParameter(name, SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue };
        }

        public string GetParameterName(string name, int index)
        {
            return new ParameterName(name, index).ToString();
        }
    }
}
