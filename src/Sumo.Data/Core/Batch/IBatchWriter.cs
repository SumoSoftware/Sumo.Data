using Sumo.Data.Schema;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Sumo.Data.Core.Batch
{
    public interface IBatchWriter
    {
        void Init(Table table, DbConnection connection);

        void Begin();

        void Execute(object[] items);

        void End();
    }
}
