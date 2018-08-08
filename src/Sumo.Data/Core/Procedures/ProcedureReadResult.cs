using System;
using System.Data;

namespace Sumo.Data
{
    public interface IProcedureReadResult
    {
        DataSet DataSet { get; }
        long ReturnValue { get; }
    }

    public class ProcedureReadResult : IProcedureReadResult
    {
        public ProcedureReadResult(DataSet dataSet, long returnValue)
        {
            DataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
            ReturnValue = returnValue;
        }

        public DataSet DataSet { get; }
        public long ReturnValue { get; } = 0;
    }
}
