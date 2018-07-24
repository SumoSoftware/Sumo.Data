using System;

namespace Sumo.Data.Names
{
    public interface IItemName: IEquatable<IItemName>
    {
        string Name { get; }
    }
}
