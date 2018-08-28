using System;
using System.Collections.Generic;
using System.Data;
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

            Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<IgnoreColumnAttribute>() == null)
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
                var name = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
                PropertyNames[i] = name;
                _properties.Add(name, property);

            }

            ReadWriteTypeCodes = new TypeCode[ReadWriteProperties.Length];
            ReadWritePropertyNames = new string[ReadWriteProperties.Length];
            for (var i = 0; i < ReadWriteProperties.Length; ++i)
            {
                var property = ReadWriteProperties[i];
                ReadWriteTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                var name = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
                ReadWritePropertyNames[i] = name;
                _readWriteProperties.Add(name, property);
            }

            ReadOnlyTypeCodes = new TypeCode[ReadOnlyProperties.Length];
            ReadOnlyPropertyNames = new string[ReadOnlyProperties.Length];
            for (var i = 0; i < ReadOnlyProperties.Length; ++i)
            {
                var property = ReadOnlyProperties[i];
                ReadOnlyTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                var propertyNameAttribute = property.GetCustomAttribute<PropertyNameAttribute>(true);
                var name = propertyNameAttribute == null ? property.Name : propertyNameAttribute.Name;
                ReadOnlyPropertyNames[i] = name;
                _readOnlyProperties.Add(name, property);
            }
        }

        private readonly static Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();
        private readonly static Dictionary<string, PropertyInfo> _readWriteProperties = new Dictionary<string, PropertyInfo>();
        private readonly static Dictionary<string, PropertyInfo> _readOnlyProperties = new Dictionary<string, PropertyInfo>();

        public readonly static PropertyInfo[] Properties;
        public readonly static PropertyInfo[] ReadWriteProperties;
        public readonly static PropertyInfo[] ReadOnlyProperties;

        public static PropertyInfo GetProperty(string name)
        {
            return _properties[name];
        }

        public static PropertyInfo GetReadWriteProperty(string name)
        {
            return _readWriteProperties[name];
        }

        public static PropertyInfo GetReadOnlyProperty(string name)
        {
            return _readOnlyProperties[name];
        }

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

            var entityPrefixAttribute = type.GetCustomAttribute<EntityPrefixAttribute>();
            var entityNameAttribute = type.GetCustomAttribute<EntityNameAttribute>();

            ProcedureName = entityNameAttribute != null ? $"[{entityNameAttribute.Name}]" : $"[{type.Name}]";
            if (entityPrefixAttribute != null && !string.IsNullOrEmpty(entityPrefixAttribute.Prefix))
            {
                ProcedureName = $"[{entityPrefixAttribute.Prefix}].{ProcedureName}";
            }

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<IgnoreParameterAttribute>() == null);

            // any property on the class will be used as an input unless it's ignored or is an output type (see above)
            InputParameters = properties
                .Where(p => p.GetGetMethod(false) != null)
                .Where(p => p.GetCustomAttribute<InputOutputParameterAttribute>(false) == null)
                .Where(p => p.GetCustomAttribute<OutputParameterAttribute>(false) == null)
                .ToArray();

            InputOutputParameters = properties
                .Where(p => p.GetGetMethod(false) != null)
                .Where(p => p.GetSetMethod(false) != null)
                .Where(p => p.GetCustomAttribute<InputOutputParameterAttribute>(false) != null)
                .Where(p => p.GetCustomAttribute<OutputParameterAttribute>(false) == null)
                .ToArray();

            OutputParameters = properties
                .Where(p => p.GetSetMethod(false) != null)
                .Where(p => p.GetCustomAttribute<InputOutputParameterAttribute>(false) == null)
                .Where(p => p.GetCustomAttribute<OutputParameterAttribute>(false) != null)
                .ToArray();

            InputTypeCodes = new TypeCode[InputParameters.Length];
            InputParameterNames = new string[InputParameters.Length];
            InputDbTypes = new DbType[InputParameters.Length];
            for (var i = 0; i < InputParameters.Length; ++i)
            {
                var property = InputParameters[i];
                InputTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                InputDbTypes[i] = property.PropertyType.ToDbType();
                var propertyNameAttribute = property.GetCustomAttribute<InputParameterAttribute>(false);
                InputParameterNames[i] = propertyNameAttribute == null || string.IsNullOrEmpty(propertyNameAttribute.Name) ? property.Name : propertyNameAttribute.Name;
            }

            OutputTypeCodes = new TypeCode[OutputParameters.Length];
            OutputParameterNames = new string[OutputParameters.Length];
            OutputDbTypes = new DbType[OutputParameters.Length];
            for (var i = 0; i < OutputParameters.Length; ++i)
            {
                var property = OutputParameters[i];
                OutputTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                OutputDbTypes[i] = property.PropertyType.ToDbType();
                var propertyNameAttribute = property.GetCustomAttribute<InputOutputParameterAttribute>(false);
                OutputParameterNames[i] = propertyNameAttribute == null || string.IsNullOrEmpty(propertyNameAttribute.Name) ? property.Name : propertyNameAttribute.Name;
            }

            InputOutputTypeCodes = new TypeCode[InputOutputParameters.Length];
            InputOutputParameterNames = new string[InputOutputParameters.Length];
            InputOutputDbTypes = new DbType[InputOutputParameters.Length];
            for (var i = 0; i < InputOutputParameters.Length; ++i)
            {
                var property = InputOutputParameters[i];
                InputOutputTypeCodes[i] = Type.GetTypeCode(property.PropertyType);
                InputOutputDbTypes[i] = property.PropertyType.ToDbType();
                var propertyNameAttribute = property.GetCustomAttribute<OutputParameterAttribute>(false);
                InputOutputParameterNames[i] = propertyNameAttribute == null || string.IsNullOrEmpty(propertyNameAttribute.Name) ? property.Name : propertyNameAttribute.Name;
            }
        }

        public readonly static PropertyInfo[] InputParameters;
        public readonly static PropertyInfo[] InputOutputParameters;
        public readonly static PropertyInfo[] OutputParameters;

        public readonly static TypeCode[] InputTypeCodes;
        public readonly static TypeCode[] InputOutputTypeCodes;
        public readonly static TypeCode[] OutputTypeCodes;

        public readonly static DbType[] InputDbTypes;
        public readonly static DbType[] InputOutputDbTypes;
        public readonly static DbType[] OutputDbTypes;

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
