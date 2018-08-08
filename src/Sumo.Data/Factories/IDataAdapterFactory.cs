using System.Data.Common;

namespace Sumo.Data
{
    public interface IDataAdapterFactory
    {
        DbDataAdapter CreateDataAdapter(DbCommand dbCommand);
    }
}
