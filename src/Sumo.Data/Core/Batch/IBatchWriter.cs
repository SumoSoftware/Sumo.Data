using Sumo.Data.Schema;
using System.Data.Common;

namespace Sumo.Data
{
    public interface IBatchWriter
    {
        void Init(Table table, DbConnection connection, bool turncate = false, bool dropTable = false);

        void Begin();

        void Execute(object[] items);

        void End();
    }
}
