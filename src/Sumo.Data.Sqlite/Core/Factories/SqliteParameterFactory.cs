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
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var type = value.GetType();
            return new SqliteParameter(name, value)
            {
                Direction = direction,
                SqliteType = type.ToSqliteType(),
                DbType = type.ToDbType()
            };
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqliteParameter(name, value)
            {
                Direction = direction,
                Size = size,
                SqliteType = value.GetType().ToSqliteType()
            };
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqliteParameter(name, type.ToSqliteType(), size)
            {
                Direction = direction,
                DbType = type
            };
        }

        public DbParameter CreateParameter(string name, DbType type, ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqliteParameter(name, type.ToSqliteType())
            {
                Direction = direction,
                DbType = type
            };
        }

        public DbParameter CreateParameter(string name, DbType type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return new SqliteParameter(name, type.ToSqliteType()) { DbType = type };
        }

        public DbParameter CreateParameter(string name, PropertyInfo property)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqliteParameter(name, property.PropertyType.ToSqliteType()){ DbType = property.PropertyType.ToDbType() };
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqliteParameter(name, property.PropertyType.ToSqliteType())
            {
                Direction = direction,
                DbType = property.PropertyType.ToDbType()
            };
        }

        public DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new SqliteParameter(name, property.PropertyType.ToSqliteType(), size)
            {
                Direction = direction,
                DbType = property.PropertyType.ToDbType()
            };
        }

        public DbParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqliteParameter(name, value)
            {
                Direction = direction,
                SqliteType = type.ToSqliteType(),
                DbType = type
            };
        }

        public DbParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction, int size)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return new SqliteParameter(name, value)
            {
                Direction = direction,
                Size = size,
                SqliteType = type.ToSqliteType(),
                DbType = type
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
