using Sumo.Data.Factories;

namespace Sumo.Data.Schema.Factories
{
    public interface ISchemaParameterFactory : IParameterFactory
    {
        string GetWriteParameterName<T>(int parameterIndex) where T : class;
    }
}
