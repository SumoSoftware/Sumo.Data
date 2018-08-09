using System;

// Interfaces and classes in the Sumo.Data.Names namespace are used for sql statement generation.
// Used in conjuction with Expressions, a query string builder need only assemble the query building blocks and call ToString()
// on the root query object to get a parameterized sql statement.
namespace Sumo.Data
{
    public interface IItemName: IEquatable<IItemName>
    {
        string Name { get; }
    }
}
