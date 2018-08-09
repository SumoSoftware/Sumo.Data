using System;

namespace Sumo.Data
{
    public interface IColumnName : IItemName, IEquatable<IColumnName>
    {
        string Alias { get; }
    }
}
