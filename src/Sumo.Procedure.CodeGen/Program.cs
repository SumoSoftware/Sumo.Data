using Sumo.Procedure.CodeGen.Application;
using Sumo.Procedure.CodeGen.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sumo.Procedure.CodeGen
{
    class Program
    {
        private static readonly string _destinationFolderParamName = "destination";
        private static readonly string _nameSpaceParamName = "namespace";

        // args
        // connection string
        // destination folder
        // root namespace
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var stopWatch = new Stopwatch();
            stopWatch.Start();


            var schema = SchemaReader.ReadSchema(AppState.ConnectionString);

            var parameters = ParseCommandLineArguments(args);
            var path = parameters[_destinationFolderParamName];
            if (!path.EndsWith(@"\")) path += @"\";

            // delete the directory to remove artifacts from the previous generation cycle
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var nameSpace = parameters[_nameSpaceParamName];
            var namespacePath = $@"{path}{nameSpace}\";
            if (!Directory.Exists(namespacePath)) Directory.CreateDirectory(namespacePath);

            Console.WriteLine($"{schema.Procedures.Count} procedures found.");

            foreach (var procedureItem in schema.Procedures)
            {
                var schemaPath = $@"{namespacePath}{procedureItem.Value.Schema}\";
                Directory.CreateDirectory(schemaPath);
                var filePath = $"{schemaPath}{procedureItem.Value.Name}.cs";

                var factory = new UnitFactory(Resources.ProcedureTemplate);
                factory.TemplateMap["{Namespace}"] = nameSpace;
                factory.TemplateMap["{Procedure}"] = procedureItem.Value.Name;
                factory.TemplateMap["{Schema}"] = procedureItem.Value.Schema;
                var builder = new StringBuilder();
                foreach (var param in procedureItem.Value.ProcedureParameters)
                {
                    builder.Append(param.ToPropertyString());
                }
                factory.TemplateMap["{Properties}"] = builder.ToString();

                var unitContent = factory.ToString();

                using (var unitFile = File.CreateText(filePath))
                {
                    unitFile.Write(unitContent);
                }
            }

            Debug.WriteLine($"Created {schema.Procedures.Count} procedure units in {stopWatch.ElapsedMilliseconds} ms.");
            Console.WriteLine($"Created {schema.Procedures.Count} procedure units in {stopWatch.ElapsedMilliseconds} ms.");
            Console.WriteLine("enter to exit");
            Console.ReadLine();
        }

        static Dictionary<string, string> ParseCommandLineArguments(string[] args)
        {
            var result = new Dictionary<string, string>(args.Length);
            for (var i = 0; i < args.Length; i += 2)
            {
                result[args[i].Remove(0, 1).ToLower()] = args[i + 1];
            }
            return result;
        }
    }
}

