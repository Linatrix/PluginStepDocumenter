using CommandLine;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using PluginStepDocumenter.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.ComponentModel.Design;

namespace PluginStepDocumenter.Application
{
    class Program
    {
        class Options
        {

            [Option('c', "connectionstring", Required = true, HelpText = "Provide a connection string to connect to target CE environment")]
            public string ConnectionString { get; set; }

            [Option('a', "assemblyname", Required = true, HelpText = "Provide an assembly name for which to document plugin steps")]
            public string AssemblyName { get; set; }

            [Option('p', "custompath", Required = false, HelpText = "Provide a custom path to place the output JSON file")]
            public string CustomPath { get; set; }

            [Option('f', "customfilename", Required = false, HelpText = "Provide a custom filename root for the output JSON file, this will default to the assembly name")]
            public string CustomFileNameRoot { get; set; }

            [Option('d', "customdateformat", Required = false, HelpText = "Provide a custom date format for the filename, default is yyyy-MM-ddTH-mm-ss-zzz")]
            public string CustomDateFormat { get; set; }
        }

        private static IOrganizationService _service;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(@"
  _____  _             _          _____ _               _____                                        _            
 |  __ \| |           (_)        / ____| |             |  __ \                                      | |           
 | |__) | |_   _  __ _ _ _ __   | (___ | |_ ___ _ __   | |  | | ___   ___ _   _ _ __ ___   ___ _ __ | |_ ___ _ __ 
 |  ___/| | | | |/ _` | | '_ \   \___ \| __/ _ \ '_ \  | |  | |/ _ \ / __| | | | '_ ` _ \ / _ \ '_ \| __/ _ \ '__|
 | |    | | |_| | (_| | | | | |  ____) | ||  __/ |_) | | |__| | (_) | (__| |_| | | | | | |  __/ | | | ||  __/ |   
 |_|    |_|\__,_|\__, |_|_| |_| |_____/ \__\___| .__/  |_____/ \___/ \___|\__,_|_| |_| |_|\___|_| |_|\__\___|_|   
                  __/ |                        | |                                                                
                 |___/                         |_|                                                               ");

            Console.WriteLine("Plugin Step Documenter v" + Assembly.GetEntryAssembly().GetName().Version + "\tLibrary v" + Assembly.GetAssembly(typeof(PluginStepDocumentBuilder)).GetName().Version);
            
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                Parser.Default.ParseArguments<Options>(args)
                       .WithParsed<Options>(o =>
                       {
                           DoWork(o);
                       });
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unexpected error caught with message: {ex.Message}");
            }

            Console.WriteLine("Any Key To Exit...");
            Console.ReadKey();
        }

        private static void DoWork(Options arguments)
        {
            string path = "";
            string fileNameRoot = arguments.AssemblyName;
            string fileNameDate = DateTime.Now.ToString("yyyy-MM-ddTH-mm-ss-ff");

            if (!string.IsNullOrEmpty(arguments.CustomPath))
            {
                Console.WriteLine($"Path provided: {arguments.CustomPath}");
                path = arguments.CustomPath;

                if(!path.EndsWith("/"))
                    path += "/";
            }

            if (!string.IsNullOrEmpty(arguments.CustomFileNameRoot))
            {
                Console.WriteLine($"File Name Root Provided: {arguments.CustomFileNameRoot}");
                fileNameRoot = arguments.CustomFileNameRoot;
            }

            if (!string.IsNullOrEmpty(arguments.CustomDateFormat))
            {
                Console.WriteLine($"Date Format Provided: {arguments.CustomDateFormat}");
                fileNameDate = DateTime.Now.ToString(arguments.CustomDateFormat);
            }

            _service = CeConnectionHelper.GetCeService(arguments.ConnectionString);

            if (CeConnectionHelper.TestConnection(_service))
            {
                string json = PluginStepDocumentBuilder.BuildPluginDocumentation(_service, arguments.AssemblyName);

                using(StreamWriter streamWriter = new StreamWriter($"{path}{fileNameRoot}{fileNameDate}.json"))
                {
                    streamWriter.Write(json);
                }
            }
        }
    }
}
