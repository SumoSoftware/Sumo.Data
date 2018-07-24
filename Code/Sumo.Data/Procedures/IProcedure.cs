using System;

namespace Sumo.Data.Procedures
{
    public interface IProcedure : IDisposable
    {
        bool Prepare<P>(P procedureParams) where P : class;
        void SetParameterValues<P>(P procedureParams) where P : class;
    }
}
