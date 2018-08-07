using Sumo.Data.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Sumo.Data.Types
{
    public static class TypeInfoCache<T> where T : class
    {
        static TypeInfoCache()
        {
            var type = typeof(T);
            FullName = type.FullName;
            Name = type.Name;

            Properties = type.GetProperties()
                .Where(p => p.GetCustomAttribute<IgnorePropertyAttribute>() == null)
                .ToArray();

            ReadWriteProperties = Properties
                .Where(p => p.GetSetMethod(false) != null)
                .Where(p => p.GetGetMethod(false) != null)
                .ToArray();

            ReadOnlyProperties = Properties
                .Where(p => p.GetGetMethod(false) != null && p.GetSetMethod(false) == null)
                .ToArray();

            TypeCodes = new TypeCode[Properties.Length];
            for(var i=0;i< Properties.Length; ++i)
            {
                TypeCodes[i] = Type.GetTypeCode(Properties[i].PropertyType);
            }

            ReadWriteTypeCodes = new TypeCode[ReadWriteProperties.Length];
            for (var i = 0; i < ReadWriteProperties.Length; ++i)
            {
                ReadWriteTypeCodes[i] = Type.GetTypeCode(ReadWriteProperties[i].PropertyType);
            }

            ReadOnlyTypeCodes = new TypeCode[ReadOnlyProperties.Length];
            for (var i = 0; i < ReadOnlyProperties.Length; ++i)
            {
                ReadOnlyTypeCodes[i] = Type.GetTypeCode(ReadOnlyProperties[i].PropertyType);
            }
        }

        public readonly static PropertyInfo[] Properties;
        public readonly static PropertyInfo[] ReadWriteProperties;
        public readonly static PropertyInfo[] ReadOnlyProperties;
        public readonly static TypeCode[] TypeCodes;
        public readonly static TypeCode[] ReadWriteTypeCodes;
        public readonly static TypeCode[] ReadOnlyTypeCodes;
        public readonly static string FullName;
        public readonly static string Name;
    }

    public static class ProcedureParametersTypeInfoCache<T> where T : class
    {
        static ProcedureParametersTypeInfoCache() 
        {
            var type = typeof(T);
            FullName = type.FullName;
            Name = type.Name;

            var prefix = type.GetCustomAttribute<EntityPrefixAttribute>();
            var suffix = type.GetCustomAttribute<EntitySuffixAttribute>();
            var name = type.GetCustomAttribute<EntityNameAttribute>();

            ProcedureName = name != null ? $"[{name}]" : $"[{type.Name}]";
            if (prefix != null && !string.IsNullOrEmpty(prefix.Prefix))
            {
                ProcedureName = $"[{prefix.Prefix}].{ProcedureName}";
            }

            var properties = type.GetProperties()
                .Where(p => p.GetCustomAttribute<IgnorePropertyAttribute>() == null);

            InputParameters = properties
                .Where(p => p.GetGetMethod(false) != null)
                .Where(p => p.GetCustomAttribute<OutputParameterAttribute>() == null)
                .Where(p => p.GetCustomAttribute<InputOutputParameterAttribute>() == null)
                .ToArray();

            InputOutputParameters = properties
                .Where(p => p.GetGetMethod(false) != null)
                .Where(p => p.GetSetMethod(false) != null)
                .Where(p => p.GetCustomAttribute<InputOutputParameterAttribute>() != null)
                .Where(p => p.GetCustomAttribute<OutputParameterAttribute>() == null)
                .ToArray();

            OutputParameters = properties
                .Where(p => p.GetSetMethod(false) != null)
                .Where(p => p.GetCustomAttribute<OutputParameterAttribute>() != null)
                .Where(p => p.GetCustomAttribute<InputOutputParameterAttribute>() == null)
                .ToArray();

            InputTypeCodes = new TypeCode[InputParameters.Length];
            for (var i = 0; i < InputParameters.Length; ++i)
            {
                InputTypeCodes[i] = Type.GetTypeCode(InputParameters[i].PropertyType);
            }

            OutputTypeCodes = new TypeCode[OutputParameters.Length];
            for (var i = 0; i < OutputParameters.Length; ++i)
            {
                OutputTypeCodes[i] = Type.GetTypeCode(OutputParameters[i].PropertyType);
            }

            InputOutputTypeCodes = new TypeCode[InputOutputParameters.Length];
            for (var i = 0; i < InputOutputParameters.Length; ++i)
            {
                InputOutputTypeCodes[i] = Type.GetTypeCode(InputOutputParameters[i].PropertyType);
            }
        }

        public readonly static PropertyInfo[] InputParameters;
        public readonly static PropertyInfo[] InputOutputParameters;
        public readonly static PropertyInfo[] OutputParameters;
        public readonly static TypeCode[] InputTypeCodes;
        public readonly static TypeCode[] InputOutputTypeCodes;
        public readonly static TypeCode[] OutputTypeCodes;
        public readonly static string ProcedureName;
        public readonly static string FullName;
        public readonly static string Name;
    }

    public static class QueryParametersTypeInfoCache<T> where T : class
    {
        static QueryParametersTypeInfoCache()
        {
            FullName = TypeInfoCache<T>.FullName;
            Name = TypeInfoCache<T>.Name;

            Parameters = TypeInfoCache<T>.Properties
                .Where(p => p.GetGetMethod(false) != null)
                .ToArray();

            TypeCodes = new TypeCode[Parameters.Length];
            for (var i = 0; i < Parameters.Length; ++i)
            {
                TypeCodes[i] = Type.GetTypeCode(Parameters[i].PropertyType);
            }
        }

        public readonly static PropertyInfo[] Parameters;
        public readonly static TypeCode[] TypeCodes;
        public readonly static string FullName;
        public readonly static string Name;
    }
}
