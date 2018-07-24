using System;

namespace Sumo.Data.Names
{
    public interface IColumnName : IItemName, IEquatable<IColumnName>
    {
        string Alias { get; }
    }
}
