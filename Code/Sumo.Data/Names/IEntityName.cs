using System;

namespace Sumo.Data.Names
{
    public interface IEntityName : IItemName, IEquatable<IEntityName>
    {
        string Schema { get; }
    }
}
