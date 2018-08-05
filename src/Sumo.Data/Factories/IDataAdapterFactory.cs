using System.Data.Common;

namespace Sumo.Data.Factories
{
    public interface IDataAdapterFactory
    {
        DbDataAdapter CreateDataAdapter(DbCommand dbCommand);
    }
}
