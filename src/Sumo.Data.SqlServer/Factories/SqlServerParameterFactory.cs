using Sumo.Data.Names;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sumo.Data.Factories.SqlServer
{
    public class SqlServerParameterFactory: IParameterFactory
    {
        public DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            return new SqlParameter(name, value) { Direction = direction };
        }

        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size)
        {
            return new SqlParameter(name, value) { Direction = direction, Size = size };
        }

        public DbParameter CreateReturnParameter(string name)
        {
            return new SqlParameter(name, SqlDbType.BigInt) { Direction = ParameterDirection.ReturnValue };
        }

        public string GetParameterName(string name, int index)
        {
            return new ParameterName(name, index).ToString();
        }

        //public string GetWriteParameterName<T>(int parameterIndex) where T : class
        //{
        //    return SqlServerEntityInfoCache<T>.EntityWriteParameterNames[parameterIndex];
        //}
    }
}
