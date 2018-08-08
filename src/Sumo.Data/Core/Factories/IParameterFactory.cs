using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Sumo.Data
{
    public interface IParameterFactory
    {
        DbParameter CreateParameter(string name, DbType type);

        DbParameter CreateParameter(string name, DbType type, ParameterDirection direction);

        DbParameter CreateParameter(string name, DbType type, ParameterDirection direction, int size);

        DbParameter CreateParameter(string name, object value, ParameterDirection direction);

        DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size);

        DbParameter CreateParameter(string name, PropertyInfo property);

        DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction);

        DbParameter CreateParameter(string name, PropertyInfo property, ParameterDirection direction, int size);

        DbParameter CreateReturnParameter(string name);

        string GetParameterName(string name, int index);
    }
}
