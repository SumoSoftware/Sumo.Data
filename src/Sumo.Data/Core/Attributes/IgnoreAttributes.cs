using System;

namespace Sumo.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreColumnAttribute : DataAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreParameterAttribute : DataAttribute
    {
    }
}
