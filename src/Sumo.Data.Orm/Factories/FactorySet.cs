using Sumo.Data.Schema;
using System;

namespace Sumo.Data.Orm
{
    public class FactorySet : IFactorySet
    {
        public FactorySet(
            IDataComponentFactory dataProviderFactory,
            ISchemaParameterNames schemaParameterFactory,
            IScriptBuilder scriptBuilder,
            ISqlStatementBuilder sqlStatementBuilder) : base()
        {
            DataProviderFactory = dataProviderFactory ?? throw new ArgumentNullException(nameof(dataProviderFactory));
            SchemaParameterFactory = schemaParameterFactory ?? throw new ArgumentNullException(nameof(schemaParameterFactory));
            ScriptBuilder = scriptBuilder ?? throw new ArgumentNullException(nameof(scriptBuilder));
            SqlStatementBuilder = sqlStatementBuilder ?? throw new ArgumentNullException(nameof(sqlStatementBuilder));
        }

        public IDataComponentFactory DataProviderFactory { get; }
        public ISchemaParameterNames SchemaParameterFactory { get; }
        public IScriptBuilder ScriptBuilder { get; }
        public ISqlStatementBuilder SqlStatementBuilder { get; }
    }
}
