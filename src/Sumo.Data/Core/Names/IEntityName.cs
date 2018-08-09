using System;

namespace Sumo.Data
{
    public interface IEntityName : IItemName, IEquatable<IEntityName>
    {
        string Schema { get; }
    }
}
