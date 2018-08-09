using System;

namespace Sumo.Data
{
    public interface IParameterName : IItemName, IEquatable<IParameterName>
    {
        int Index { get; }
    }
}
