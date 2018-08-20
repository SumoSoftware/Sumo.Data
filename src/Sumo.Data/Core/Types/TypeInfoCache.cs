using System;
using System.Linq;
using System.Reflection;

namespace Sumo.Data
{
    internal static class TypeInfoCache<T> where T : class
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
            PropertyNames = new string[Properties.Length];
            for (var i = 0; i < Properties.Length; ++i)
            {
                var property = Properties[i];
                TypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                PropertyNames[i] = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
            }

            ReadWriteTypeCodes = new TypeCode[ReadWriteProperties.Length];
            ReadWritePropertyNames = new string[ReadWriteProperties.Length];
            for (var i = 0; i < ReadWriteProperties.Length; ++i)
            {
                var property = ReadWriteProperties[i];
                ReadWriteTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                ReadWritePropertyNames[i] = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
            }

            ReadOnlyTypeCodes = new TypeCode[ReadOnlyProperties.Length];
            ReadOnlyPropertyNames = new string[ReadOnlyProperties.Length];
            for (var i = 0; i < ReadOnlyProperties.Length; ++i)
            {
                var property = ReadOnlyProperties[i];
                ReadOnlyTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                ReadOnlyPropertyNames[i] = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
            }
        }

        public readonly static PropertyInfo[] Properties;
        public readonly static PropertyInfo[] ReadWriteProperties;
        public readonly static PropertyInfo[] ReadOnlyProperties;

        public readonly static TypeCode[] TypeCodes;
        public readonly static TypeCode[] ReadWriteTypeCodes;
        public readonly static TypeCode[] ReadOnlyTypeCodes;

        public readonly static string[] PropertyNames;
        public readonly static string[] ReadWritePropertyNames;
        public readonly static string[] ReadOnlyPropertyNames;

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

            var prefixAttribute = type.GetCustomAttribute<EntityPrefixAttribute>();
            var nameAttribute = type.GetCustomAttribute<EntityNameAttribute>();

            ProcedureName = nameAttribute != null ? $"[{nameAttribute.Name}]" : $"[{type.Name}]";
            if (prefixAttribute != null && !string.IsNullOrEmpty(prefixAttribute.Prefix))
            {
                ProcedureName = $"[{prefixAttribute.Prefix}].{ProcedureName}";
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
            InputParameterNames = new string[InputParameters.Length];
            for (var i = 0; i < InputParameters.Length; ++i)
            {
                var property = InputParameters[i];
                InputTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                InputParameterNames[i] = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
            }

            OutputTypeCodes = new TypeCode[OutputParameters.Length];
            OutputParameterNames = new string[OutputParameters.Length];
            for (var i = 0; i < OutputParameters.Length; ++i)
            {
                var property = OutputParameters[i];
                OutputTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                OutputParameterNames[i] = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
            }

            InputOutputTypeCodes = new TypeCode[InputOutputParameters.Length];
            InputOutputParameterNames = new string[InputOutputParameters.Length];
            for (var i = 0; i < InputOutputParameters.Length; ++i)
            {
                var property = InputOutputParameters[i];
                InputOutputTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                InputOutputParameterNames[i] = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
            }
        }

        public readonly static PropertyInfo[] InputParameters;
        public readonly static PropertyInfo[] InputOutputParameters;
        public readonly static PropertyInfo[] OutputParameters;

        public readonly static TypeCode[] InputTypeCodes;
        public readonly static TypeCode[] InputOutputTypeCodes;
        public readonly static TypeCode[] OutputTypeCodes;

        public readonly static string[] InputParameterNames;
        public readonly static string[] InputOutputParameterNames;
        public readonly static string[] OutputParameterNames;

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
