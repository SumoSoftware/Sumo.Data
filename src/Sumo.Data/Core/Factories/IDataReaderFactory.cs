using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Sumo.Data.Core.Factories
{
    public interface IDataReaderFactory
    {
        IDataReader CreateDataAdapter(DbCommand dbCommand);
    }
}
