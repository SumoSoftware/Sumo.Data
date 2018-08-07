using System.Collections.Generic;

namespace Sumo.Data.Schema
{
    public interface ISqlStatementBuilder
    {
        string GetExistsStatement<T>() where T : class;
        string GetInsertStatement<T>() where T : class;
        string GetSelectStatement<T>(Dictionary<string, object> parameters) where T : class;
    }
}