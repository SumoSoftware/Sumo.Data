using System.Collections.Generic;

namespace Sumo.Data
{
    public interface IPreparable
    {
        bool Prepare(string sql, Dictionary<string, object> parameters = null);
        void SetParameterValues(string sql, Dictionary<string, object> parameters = null);

        //todo: is there a way to support these methods?
        //bool Prepare<P>(P procedureParams) where P : class;
        //void SetParameterValues<P>(P procedureParams) where P : class;
    }
}
