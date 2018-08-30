﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sumo.Procedure.CodeGen.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sumo.Procedure.CodeGen.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT 
        ///	SPECIFIC_SCHEMA as [Schema], 
        ///	SPECIFIC_NAME as [Procedure], 
        ///	RIGHT(PARAMETER_NAME, LEN(PARAMETER_NAME) - 1) as [Name], 
        ///	DATA_TYPE as [DataType], 
        ///	PARAMETER_MODE as [Direction], 
        ///	ORDINAL_POSITION as [Order], 
        ///	CHARACTER_MAXIMUM_LENGTH as [MaxLength], 
        ///	CHARACTER_SET_NAME as [Encoding]
        ///FROM INFORMATION_SCHEMA.PARAMETERS
        ///where IS_RESULT = &apos;NO&apos;
        ///order by SPECIFIC_SCHEMA, SPECIFIC_NAME, ORDINAL_POSITION.
        /// </summary>
        internal static string GetProcedureParametersSql {
            get {
                return ResourceManager.GetString("GetProcedureParametersSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [SPECIFIC_NAME] as [Name], [specific_schema] as [Schema]  FROM INFORMATION_SCHEMA.ROUTINES
        ///where ROUTINE_TYPE != &apos;FUNCTION&apos;
        ///order by 2, 1.
        /// </summary>
        internal static string GetProceduresSql {
            get {
                return ResourceManager.GetString("GetProceduresSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [schema_name] as [Name] FROM INFORMATION_SCHEMA.SCHEMATA
        ///where 
        ///	[schema_owner] = &apos;dbo&apos;
        ///	and [schema_name] not like &apos;db_%&apos;
        ///order by 1.
        /// </summary>
        internal static string GetSchemasSql {
            get {
                return ResourceManager.GetString("GetSchemasSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [table_name] as [Name], [table_schema] as [Schema]
        ///FROM INFORMATION_SCHEMA.TABLES
        ///where 
        ///	[table_schema] != &apos;sys&apos;
        ///	and [table_type] = &apos;BASE TABLE&apos;
        ///order by 2, 1.
        /// </summary>
        internal static string GetTablesSql {
            get {
                return ResourceManager.GetString("GetTablesSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using Sumo.Data.Attributes;
        ///using System;
        ///
        ///namespace {Namespace}.{Schema}
        ///{
        ///    public sealed class {Procedure}
        ///    {
        ///{Properties}
        ///    }
        ///}.
        /// </summary>
        internal static string ProcedureTemplate {
            get {
                return ResourceManager.GetString("ProcedureTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {Attribute}
        ///public {Type} {Name} { get; set; }.
        /// </summary>
        internal static string PropertyTemplate {
            get {
                return ResourceManager.GetString("PropertyTemplate", resourceCulture);
            }
        }
    }
}
