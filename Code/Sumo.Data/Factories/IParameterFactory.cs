using System.Data;
using System.Data.Common;

namespace Sumo.Data.Factories
{
    public interface IParameterFactory
    {
        DbParameter CreateParameter(string name, object value, ParameterDirection direction, int size);

        DbParameter CreateParameter(string name, object value, ParameterDirection direction);

        DbParameter CreateReturnParameter(string name);
    }
}
