using Sumo.Data.Factories;

namespace Sumo.Data.Schema.Factories
{
    public interface ISchemaParameterFactory : IParameterFactory
    {
        /// <summary>
        /// this method provides an optimization for access of the EntityInfoCache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterIndex"></param>
        /// <returns>the database specific parameter name</returns>
        string GetWriteParameterName<T>(int parameterIndex) where T : class;
    }
}
