using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Sumo.Data.Sqlite
{
    public class SqliteParameterFactory : IParameterFactory
    {
        public DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqliteParameter(name, value)
            {
                Direction = direction,
                SqliteType = value.GetType().ToDbType().ToSqliteType()
            };
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqliteParameter(name, value)
            {
                Direction = direction,
                Size = size,
                SqliteType = value.GetType().ToDbType().ToSqliteType()
            };
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction, int size)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqliteParameter(name, type.ToSqliteType(), size)
            {
                Direction = direction
            };
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqliteParameter(name, type.ToSqliteType())
            {
                Direction = direction
            };
        }

        public DbParameter CreateParameter(string name, DbType type)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqliteParameter(name, type.ToSqliteType());
        }

        public DbParameter CreateParameter(string name, PropertyInfo property)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqliteParameter(name, property.PropertyType.ToSqliteType());
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqliteParameter(name, property.PropertyType.ToSqliteType())
            {
                Direction = direction
            };
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction, int size)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqliteParameter(name, property.PropertyType.ToSqliteType(), size)
            {
                Direction = direction
            };
        }

        public DbParameter CreateReturnParameter(string name)
        {
            return new SqliteParameter(name, SqliteType.Integer) { Direction = ParameterDirection.ReturnValue };
        }

        public string GetParameterName(string name, int index)
        {
            return new ParameterName(name, index).ToString();
        }
    }
}
