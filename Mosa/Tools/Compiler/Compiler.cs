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
using System.Text;

using Mosa.Platforms.x86;
using Mosa.Runtime.CompilerFramework;
using Mosa.Tools.Compiler.Boot;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Class containing the Compiler.
    /// </summary>
    public class Compiler
    {
        #region Fields

        /// <summary>
        /// Holds the stage responsible for the architecture.
        /// </summary>
        private ArchitectureSelector architectureStage;

        /// <summary>
        /// Holds the stage responsible for the linker/binary format.
        /// </summary>
        private LinkerFormatSelector linkerStage;

        /// <summary>
        /// Holds the stage responsible for the boot format.
        /// </summary>
        private BootFormatSelector bootFormatStage;

        /// <summary>
        /// Holds a list of input files.
        /// </summary>
        private List<FileInfo> inputFiles;

        /// <summary>
        /// Determines if the file is executable.
        /// </summary>
        private bool isExecutable;

        /// <summary>
        /// Holds the name of the map file to generate.
        /// </summary>
        private string mapFile;

        /// <summary>
        /// Holds a reference to the OptionSet used for option parsing.
        /// </summary>
        private OptionSet optionSet;

        /// <summary>
        /// A string holding a simple usage description.
        /// </summary>
        private readonly string usageString;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Compiler class.
        /// </summary>
        public Compiler()
        {
            usageString = "Usage: mosacl -o outputfile --arch=[x86|x64] --format=[ELF|PE] --boot=[mb0.7] {additional options} inputfiles";
            optionSet = new OptionSet();

            this.linkerStage = new LinkerFormatSelector();
            this.bootFormatStage = new BootFormatSelector();
            this.architectureStage = new ArchitectureSelector();

            #region Setup general options
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
                        this.ShowHelp();
                        Environment.Exit(0);
                    }
                });

            optionSet.Add(
                "map=",
                "Generate a map {file} of the produced binary.",
                this.SetMapFile
               );

            #endregion

            this.linkerStage.AddOptions(optionSet);
            this.bootFormatStage.AddOptions(optionSet);
            this.architectureStage.AddOptions(optionSet);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Runs the command line parser and the compilation process.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public void Run(string[] args)
        {
            // always print header with version information
            Console.WriteLine("MOSA AOT Compiler, Version 0.1 'Wake'");
            Console.WriteLine("(C) 2008 by the MOSA Project, Licensed under the new BSD license.");
            Console.WriteLine();

            try
            {
                if (args == null || args.Length == 0)
                {
                    // no arguments are specified
                    ShowShortHelp();
                    return;
                }

                List<string> files = optionSet.Parse(args);

                if (files.Count == 0)
                {
                    throw new OptionException("No input file(s) specified.", String.Empty);
                }

                inputFiles = new List<FileInfo>();

                for (int i = 0; i < files.Count; i++)
                {
                    if (!File.Exists(files[i]))
                    {
                        throw new OptionException(String.Format("Input file or option '{0}' doesn't exist.", files[i]), String.Empty);
                    }

                    FileInfo file  = new FileInfo(files[i]);
                    if (file.Extension.ToLower() == ".exe")
                    {
                        if (isExecutable)
                        {
                            // there are more than one exe files in the list
                            throw new OptionException("Multiple executables aren't allowed.", String.Empty);
                        }

                        isExecutable = true;
                    }

                    inputFiles.Add(file);
                }

                // Process boot format:
                // Boot format only matters if it's an executable
                // Process this only now, because input files must be known
                if (isExecutable)
                {
                    if (!bootFormatStage.IsImplementationSelected)
                    {
                        throw new OptionException("No boot format specified.", "boot");
                    }
                }
                else if (bootFormatStage.IsImplementationSelected)
                {
                    Console.WriteLine("Warning: Ignoring boot format, because target is not an executable.");
                    Console.WriteLine();
                }

                // Check for missing options
                if (!linkerStage.IsImplementationSelected)
                {
                    throw new OptionException("No binary format specified.", "arch");
                }

                if (String.IsNullOrEmpty(this.linkerStage.OutputFile))
                {
                    throw new OptionException("No output file specified.", "o");
                }

                if (!architectureStage.IsImplementationSelected)
                {
                    throw new OptionException("No architecture specified.", "arch");
                }
            }
            catch (OptionException e)
            {
                ShowError(e.Message);
                return;
            }

            Compile();
        }

        /// <summary>
        /// Returns a string representation of the current options.
        /// </summary>
        /// <returns>A string containing the options.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Output file: ").AppendLine(this.linkerStage.OutputFile);
            sb.Append("Input file(s): ").AppendLine(String.Join(", ", new List<string>(GetInputFileNames()).ToArray()));
            sb.Append("Architecture: ").AppendLine(architectureStage.Name);
            sb.Append("Binary format: ").AppendLine(linkerStage.Name);
            sb.Append("Boot format: ").AppendLine(bootFormatStage.Name);
            sb.Append("Is executable: ").AppendLine(isExecutable.ToString());
            return sb.ToString();
        }

        #endregion Public Methods

        #region Private Methods

        private void Compile()
        {
            // TODO: compilation has to start here, showing parsing results

            Console.WriteLine(this.ToString());

            //CompilerScheduler.Setup(2);

            string assembly = this.inputFiles[0].FullName, path;

            IArchitecture architecture = Architecture.CreateArchitecture(ArchitectureFeatureFlags.AutoDetect);
            ObjectFileBuilderBase objfile = architecture.GetObjectFileBuilders()[0];
            using (CompilationRuntime cr = new CompilationRuntime()) {
                path = Path.GetDirectoryName(assembly);
                cr.AssemblyLoader.AppendPrivatePath(path);
                AotCompiler.Compile(architecture, assembly, objfile);
            }

            //CompilerScheduler.Wait();
        }

        /// <summary>
        /// Sets the map file to generate after linking.
        /// </summary>
        /// <param name="file">The map file.</param>
        private void SetMapFile(string file)
        {
            // Optional!
            mapFile = file;
        }

        /// <summary>
        /// Gets a list of input file names.
        /// </summary>
        private IEnumerable<string> GetInputFileNames()
        {
            foreach (FileInfo file in inputFiles)
            {
                yield return file.FullName;
            }
        }

        private void ShowError(string message)
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.Write ("Error: ");
            Console.WriteLine (message);
            Console.WriteLine();
            Console.WriteLine ("Run 'mosacl --help' for more information.");
        }

        private void ShowShortHelp()
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.WriteLine ("Run 'mosacl --help' for more information.");
        }

        private void ShowHelp()
        {
            Console.WriteLine(usageString);
            Console.WriteLine();
            Console.WriteLine("Options:");
            this.optionSet.WriteOptionDescriptions(Console.Out);
        }

        #endregion Private Methods
    }
}
