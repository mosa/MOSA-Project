/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License
 * with restrictions to the license beneath, concering
 * the use of the CommandLine assembly.
 *
 * Authors:
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
 */

using System;
using System.Collections.Generic;
using System.IO;

using NDesk.Options;

using Mosa.Platforms.x86;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("MOSA Compiler, Version X.Y.Z");
            Console.WriteLine("(C) 2008 by the MOSA Project, Licensed with the new BSD license.");
            Console.WriteLine();
            
            if (args.Length == 0)
            {
                ShowShortHelp();
                return;
            }
            
            List<string> inputFiles;
            bool showVersion = false;
            bool showHelp = false;
            string outputFile = null;
            string arch = null;
            string binaryFormat = null;
            string bootFormat = null;
            CompileOptions options = new CompileOptions();
            
            OptionSet optionSet = new OptionSet();
            optionSet.Add(
                "v|version",
                "Specifies the MOSA version.",
                delegate(string v)
                {
                    showVersion = (v != null);
                });
            
            optionSet.Add(
                "h|?|help",
                "Displays the full set of available options.",
                delegate(string v)
                {
                    showHelp = (v != null);
                });
            
            optionSet.Add(
                "o=",
                "The name of the output file.",
                delegate(string v)
                {
                    outputFile = v;
                });
            
            optionSet.Add(
                "a|arch=",
                "Selects one of the MOSA architectures to compile for.",
                delegate(string v)
                {
                    arch = v;
                });
            
             optionSet.Add(
                "f|format=",
                "Selects the format of the binary file to create.",
                delegate(string v)
                {
                    binaryFormat = v;
                });
            
            optionSet.Add(
                "b|boot=",
                "Specifies the bootable format of the produced binary.",
                delegate(string v)
                {
                    bootFormat = v;
                });
            
            try
            {
                inputFiles = optionSet.Parse(args);
                
                if (showHelp)
                {
                    ShowHelp(optionSet);
                    return;
                }
                
                if (showVersion)
                {
                    Console.WriteLine("****TODO****"); // TODO: version information
                    return;
                }
                
                options.SetInputFiles(inputFiles);
                options.SetOutputFile(outputFile);
                options.SetArchitecture(arch);
                options.SetBinaryFormat(binaryFormat);
                options.SetBootFormat(bootFormat);
            }
            catch (OptionException e)
            {
                ShowError(e.Message);
                return;
            }
            
            // TODO: compilation has to start here, showing parsing results instead
            Console.WriteLine(options.ToString());
            
            //CompilerScheduler.Setup(2);

            string assembly = options.InputFiles[0], path;

            IArchitecture architecture = Architecture.CreateArchitecture(ArchitectureFeatureFlags.AutoDetect);
            ObjectFileBuilderBase objfile = architecture.GetObjectFileBuilders()[0];
            using (CompilationRuntime cr = new CompilationRuntime())
            {
                path = Path.GetDirectoryName(assembly);
                cr.AssemblyLoader.AppendPrivatePath(path);
                AotCompiler.Compile(architecture, assembly, objfile);
            }

            //CompilerScheduler.Wait();
        }
        
        static string usageString = "Usage: mosacl -o outputfile --arch=[x86,x64] --format=[elf,pe] --boot=[mb0.7] inputfiles";
        
        /// <summary>
        /// Shows the specified error message.
        /// </summary>
        /// <param name="message">A string containing the message.</param>
        static void ShowError(string message)
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.Write ("mosacl: ");
            Console.WriteLine (message);
            Console.WriteLine();
            Console.WriteLine ("Run 'mosacl --help' for more information.");
        }
        
        static void ShowShortHelp()
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.WriteLine ("Run 'mosacl --help' for more information.");
        }
        
        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
