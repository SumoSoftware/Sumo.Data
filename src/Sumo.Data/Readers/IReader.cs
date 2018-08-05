using System;
using System.Collections.Generic;

namespace Sumo.Data.Readers
{
    public interface IReader : IDisposable
    {
        bool Prepare(string sql, Dictionary<string, object> queryParams = null);
        void SetParameterValues(string sql, Dictionary<string, object> queryParams = null);
    }
}
