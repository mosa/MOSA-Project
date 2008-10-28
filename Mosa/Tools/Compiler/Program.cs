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
    /// Class containing the entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the compiler.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            // always print header with version information
            Console.WriteLine("MOSA AOT Compiler, Version 0.1 'Wake'");
            Console.WriteLine("(C) 2008 by the MOSA Project, Licensed under the new BSD license.");
            Console.WriteLine();
            
            if (args.Length == 0)
            {
                // no arguments are specified
                ShowShortHelp();
                return;
            }

            string mapFile = null;
            CompileOptions options = new CompileOptions();
            
            OptionSet optionSet = new OptionSet();
            
            #region Setup options
            // General options
            optionSet.Add(
                "v|version",
                "Display version information.",
                delegate(string v)
                {
                    if (v != null)
                    {
                        // only show header and exit
                        Environment.Exit(0);
                    }
                });
            
            optionSet.Add(
                "h|?|help",
                "Display the full set of available options.",
                delegate(string v)
                {
                    if (v != null)
                    {
                        ShowHelp(optionSet);
                        Environment.Exit(0);
                    }
                });
            
            optionSet.Add(
                "o|out=",
                "The name of the output {file}.",
                options.SetOutputFile
               );
            
            optionSet.Add(
                "a|arch=",
                "Select one of the MOSA architectures to compile for [{x86|x64}].",
                options.SetArchitecture
               );
            
            optionSet.Add(
                "f|format=",
                "Select the format of the binary file to create [{ELF|PE}].",
                options.SetBinaryFormat
               );
            
            optionSet.Add(
                "b|boot=",
                "Specify the bootable format of the produced binary [{mb0.7}].",
                options.SetBootFormat
               );

            optionSet.Add(
                "m|map=",
                "Generate a map file of the produced binary.",
                delegate(string v)
                {
                    mapFile = v;
                });
            
                       
//            // Additional options for multiboot
//            optionSet.Add(
//                "multiboot-video-mode=",
//                "Specify the video mode for multiboot [{text|graphics}].",
//                delegate(string v)
//                {
//                    // only process for multiboot 0.7
//                    if (options.BootFormat == BootFormat.Multiboot0_7)
//                    {
//                        Console.WriteLine("Multiboot video mode: " + v);
//                        Console.WriteLine();
//                    }
//                });
            
            #endregion
            
            try
            {
                options.Finish(optionSet.Parse(args));
                options.SetMapFile(mapFile);
            }
            catch (OptionException e)
            {
                ShowError(e.Message);
                return;
            }
            
            // TODO: compilation has to start here, showing parsing results instead
            Console.WriteLine(options.ToString());
            
            //CompilerScheduler.Setup(2);

            string assembly = options.InputFiles[0].FullName, path;

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
        
        static string usageString = "Usage: mosacl -o outputfile --arch=[x86|x64] --format=[ELF|PE] --boot=[mb0.7] {additional options} inputfiles";

        static void ShowError(string message)
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.Write ("Error: ");
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
