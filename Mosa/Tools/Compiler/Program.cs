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

/*
 * Copyright (c) 2005 - 2008 Giacomo Stelluti Scala
 * 
 * Permission is hereby granted, free of charge, 
 * to any person obtaining a copy of this software 
 * and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, 
 * including without limitation the rights to use, 
 * copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit 
 * persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice 
 * shall be included in all copies or substantial portions 
 * of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY 
 * OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS 
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.IO;

using CommandLine;
using CommandLine.Text;

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
            Program program = new Program(args);

            //CompilerScheduler.Setup(2);

            string assembly = program.InputFiles[0], path;

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

        /// <summary>
        /// The heading information for the console output:
        /// The application name and it's version.
        /// </summary>
        private readonly HeadingInfo headingInfo = new HeadingInfo("MOSA Compiler", "X.Y.Z");

        /// <summary>
        /// Class for the command line options.
        /// </summary>
        private sealed class Options
        {
            private readonly string usageLine = "Usage: mosacl -o outputfile --arch=[x86,x64] --format=[elf,pe] --boot=[mb0.7] inputfiles";

            [Option("o", null, HelpText = "The name of the output file.")]
            public string OutputFile = String.Empty;

            [Option("f", "format", HelpText = "Selects the format of the binary file to create.")]
            public string BinaryFormatString = String.Empty;

            [Option("a", "arch", HelpText = "Selects one of the MOSA architectures to compile for.")]
            public string ArchitectureString = String.Empty;

            [Option("b", "boot", HelpText = "Specifies the bootable format of the produced binary.")]
            public string BootFormatString = String.Empty;

            [Option("v", "version", HelpText = "Specifies the MOSA version.")]
            public bool ShowVersion = false;

            [ValueList(typeof(List<string>))]
            public IList<string> InputFiles = null;

            /// <summary>
            /// Creates a short help with usage information.
            /// </summary>
            /// <returns>The help text as string.</returns>
            public string GetShortHelp()
            {
                HelpText help = new HelpText(usageLine);
                help.AddPreOptionsLine(Environment.NewLine + "Run mosacl --help for more information.");
                return help;
            }

            /// <summary>
            /// Creates the long help with all available options.
            /// </summary>
            /// <returns>The help text as string.</returns>
            [HelpOption("h", "help", HelpText = "Displays the full set of available options.")]
            public string GetHelp()
            {
                HelpText help = new HelpText(usageLine);
                help.AddPreOptionsLine(Environment.NewLine + "Options:");
                help.AddOptions(this);
                return help;
            }
        }

        /// <summary>
        /// Available architectures.
        /// </summary>
        private enum Architectures { x86, x64 }

        /// <summary>
        /// Available binary formats
        /// </summary>
        private enum BinaryFormats { ELF, PE }

        /// <summary>
        /// Available boot formats. Use '_' instead of '.'.
        /// </summary>
        private enum BootFormats { mb0_7 }

        /// <summary>
        /// The specified architecture to compile for.
        /// </summary>
        private Architectures _architecture;

        /// <summary>
        /// The specified binary format to compile to.
        /// </summary>
        private BinaryFormats _binaryFormat;

        /// <summary>
        /// The specified file to compile to.
        /// </summary>
        private string _outputFile;

        /// <summary>
        /// The list of source files.
        /// </summary>
        private List<string> _inputFiles;

        /// <summary>
        /// Whether the 
        /// </summary>
        private bool _isExecutable = false;

        /// <summary>
        /// The specified boot format to compile for.
        /// </summary>
        private BootFormats _bootFormat;

        /// <summary>
        /// Gets the input files.
        /// </summary>
        /// <value>The input files.</value>
        public List<string> InputFiles
        {
            get
            {
                return _inputFiles;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public Program(string[] args)
        {
            // Always show heading and copyright information.
            Console.WriteLine(headingInfo);
            Console.WriteLine("(C) 2008 by the MOSA Project, Licensed with the new BSD license.");
            Console.WriteLine();

            ParseOptions(args);

            // Print parsing results. The compilation process has to be started here.
            Console.WriteLine("Architecture: " + _architecture.ToString());
            Console.WriteLine("Binary format: " + _binaryFormat.ToString());
            Console.WriteLine("Compiling to file: " + _outputFile);
            Console.WriteLine("Compiling files: " + String.Join(", ", _inputFiles.ToArray()));
            Console.WriteLine("Target: " + (_isExecutable ? ("exe (" + _bootFormat.ToString() + ")") : "lib"));
        }

        /// <summary>
        /// Setup the options for the compiler based on the command line parameters.
        /// </summary>
        /// <param name="args">The command line parameters.</param>
        private void ParseOptions(string[] args)
        {
            Options options = new Options();

            if (args.Length < 1)
            {
                // This is called when mosacl is executed without arguments
                Console.WriteLine(options.GetShortHelp());
                Environment.Exit(1);
            }

            if (Parser.ParseArguments(args, options, Console.Out))
            {
                if (options.ShowVersion)
                {
                    // if -v/--version was specified, print version information and exit.
                    Console.WriteLine("TODO: Print version information here."); // TODO.
                    Environment.Exit(0);
                }

                #region Architecture
                if (String.IsNullOrEmpty(options.ArchitectureString))
                {
                    // no architecture is specified
                    Console.WriteLine(options.GetShortHelp());
                    Environment.Exit(1);
                }

                try
                {
                    _architecture = (Architectures)Enum.Parse(typeof(Architectures), options.ArchitectureString, true);
                    if (false == Enum.IsDefined(typeof(Architectures), _architecture))
                    {
                        throw new ArgumentException();
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Unknown or unsupported architecture " + options.ArchitectureString + ".");
                    Environment.Exit(1);
                }
                #endregion

                #region Binary format
                if (String.IsNullOrEmpty(options.BinaryFormatString))
                {
                    // no binary format is specified
                    Console.WriteLine(options.GetShortHelp());
                    Environment.Exit(1);
                }

                try
                {
                    _binaryFormat = (BinaryFormats)Enum.Parse(typeof(BinaryFormats), options.BinaryFormatString, true);
                    if (false == Enum.IsDefined(typeof(BinaryFormats), _binaryFormat))
                    {
                        throw new ArgumentException();
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Unknown or unsupported binary format " + options.BinaryFormatString + ".");
                    Environment.Exit(1);
                }
                #endregion

                #region Input files
                if (null == options.InputFiles || options.InputFiles.Count < 1)
                {
                    Console.WriteLine(options.GetShortHelp());
                    Environment.Exit(1);
                }
                else
                {
                    _inputFiles = new List<string>(options.InputFiles);
                    foreach (var file in _inputFiles)
                    {
                        string extension = new FileInfo(file).Extension;
                        if (extension.ToLower() == ".exe")
                        {
                            if (_isExecutable)
                            {
                                // there are more than one exe files in the list
                                Console.WriteLine("Multiple executables aren't allowed.");
                                Environment.Exit(1);
                            }
                            _isExecutable = true;
                        }
                    }
                }
                #endregion

                #region Output file
                if (String.IsNullOrEmpty(options.OutputFile))
                {
                    // no output filename specified, use first input file for this
                    // TODO: maybe generate output filename based on input filenames?
                    Console.WriteLine(options.GetShortHelp());
                    Environment.Exit(1);
                }
                else
                {
                    _outputFile = options.OutputFile;
                }
                #endregion

                #region Boot format
                // boot format only matters if it's an executable
                if (_isExecutable)
                {
                    if (String.IsNullOrEmpty(options.BootFormatString))
                    {
                        // no boot format is specified

                        Console.WriteLine("No boot format is specified.");
                        Environment.Exit(1);
                    }

                    try
                    {
                        _bootFormat = (BootFormats)Enum.Parse(typeof(BootFormats), options.BootFormatString.Replace('.', '_'), true);
                        if (false == Enum.IsDefined(typeof(BootFormats), _bootFormat))
                        {
                            throw new ArgumentException();
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Unknown or unsupported boot format " + options.BootFormatString + ".");
                        Environment.Exit(1);
                    }
                }

                #endregion
            }
            else
            {
                Environment.Exit(1);
            }
        }
    }
}
