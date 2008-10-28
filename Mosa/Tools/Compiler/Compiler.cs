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
using System.Text;
using System.IO;

using NDesk.Options;

using Mosa.Platforms.x86;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;

using Mosa.Tools.Compiler.Boot;

namespace Mosa.Tools.Compiler
{
    #region Enums

    /// <summary>
    /// Available architectures.
    /// </summary>
    public enum TargetArchitecture
    {
        /// <summary>
        /// Architecture not defined.
        /// </summary>
        Undefined,
        
        /// <summary>
        /// The x86 architecture.
        /// </summary>
        x86,
        
        /// <summary>
        /// The x64 architecture,
        /// </summary>
        x64
    }
    
    /// <summary>
    /// Available binary formats
    /// </summary>
    public enum BinaryFormat
    {
        /// <summary>
        /// Binary format not defined.
        /// </summary>
        Undefined,
        
        /// <summary>
        /// The ELF (Executable and Linking Format) format.
        /// </summary>
        ELF,
        
        /// <summary>
        /// The PE (Portable Executable) format.
        /// </summary>
        PE
    }

    #endregion // Enums

    /// <summary>
    /// Class containing the Compiler.
    /// </summary>
    public class Compiler
    {
        // TODO: replace architecture and binaryFormat by stages (see bootFormatStage)
        
        #region Fields

        /// <summary>
        /// Determines the compilation architecture.
        /// </summary>
        private TargetArchitecture architecture;
        
        /// <summary>
        /// Determines the output binary format.
        /// </summary>
        private BinaryFormat binaryFormat;

        /// <summary>
        /// Holds the stage responsible for the boot format.
        /// </summary>
        private IAssemblyCompilerStage bootFormatStage = null;

        /// <summary>
        /// Holds a list of input files.
        /// </summary>
        private List<FileInfo> inputFiles = null;
        
        /// <summary>
        /// Determines if the file is executable.
        /// </summary>
        private bool isExecutable;

        /// <summary>
        /// Holds the name of the output file to generate.
        /// </summary>
        private string outputFile;

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
        /// Initializes a new instance of the CompileOptions class.
        /// </summary>
        public Compiler()
        {
            usageString = "Usage: mosacl -o outputfile --arch=[x86|x64] --format=[ELF|PE] --boot=[mb0.7] {additional options} inputfiles";
            architecture = TargetArchitecture.Undefined;
            binaryFormat = BinaryFormat.Undefined;
            optionSet = new OptionSet();
            
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
                        this.AddAllOptions();
                        this.ShowHelp();
                        Environment.Exit(0);
                    }
                });
            
            optionSet.Add(
                "o|out=",
                "The name of the output {file}.",
                this.SetOutputFile
               );
            
            optionSet.Add(
                "a|arch=",
                "Select one of the MOSA architectures to compile for [{x86|x64}].",
                this.SetArchitecture
               );
            
            optionSet.Add(
                "f|format=",
                "Select the format of the binary file to create [{ELF|PE}].",
                this.SetBinaryFormat
               );
            
            optionSet.Add(
                "b|boot=",
                "Specify the bootable format of the produced binary [{mb0.7}].",
                this.SetBootFormat
               );

            optionSet.Add(
                "map=",
                "Generate a map {file} of the produced binary.",
                this.SetMapFile
               );
            #endregion
        }

        #endregion Constructors

        #region Properties
        
        /// <summary>
        /// Gets a list of input file names.
        /// </summary>
        private IEnumerable<string> InputFileNames
        {
            get
            {
                foreach (FileInfo file in inputFiles)
                {
                    yield return file.FullName;
                }
            }
        }

        #endregion Properties

        #region Public Methods
        
        /// <summary>
        /// Runs the parser and the compilation process.
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
                if (args.Length == 0)
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
                    if (bootFormatStage == null)
                    {
                        throw new OptionException("No boot format specified or boot format unknown or not supported.", "boot");
                    }
                }
                else if (bootFormatStage != null)
                {
                    Console.WriteLine("Warning: Ignoring boot format, because target is not an executable.");
                    Console.WriteLine();
                }
                
                // Check for missing options
                if (String.IsNullOrEmpty(outputFile))
                {
                    throw new OptionException("No output file specified.", "o");
                }
                
                if (binaryFormat == BinaryFormat.Undefined)
                {
                    throw new OptionException("No binary format specified.", "format");
                }
                
                if (architecture == TargetArchitecture.Undefined)
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
            sb.Append("Output file: ").AppendLine(outputFile);
            sb.Append("Input file(s): ").AppendLine(String.Join(", ", new List<string>(InputFileNames).ToArray()));
            sb.Append("Architecture: ").AppendLine(architecture.ToString());
            sb.Append("Binary format: ").AppendLine(binaryFormat.ToString());
            sb.Append("Boot format: ").AppendLine(bootFormatStage.ToString());
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
        /// Sets the architecture to compile for.
        /// </summary>
        /// <param name="arch">The architecture as a string.</param>
        private void SetArchitecture(string arch)
        {
            try
            {
                architecture = EnumParse<TargetArchitecture>(arch);
            }
            catch (ArgumentException)
            {
                throw new OptionException(String.Format("Unknown or unsupported architecture {0}.", arch), "arch");
            }
        }

        /// <summary>
        /// Sets the binary format to compile to.
        /// </summary>
        /// <param name="format">The binary format as a string.</param>
        private void SetBinaryFormat(string format)
        {
            try
            {
                binaryFormat = EnumParse<BinaryFormat>(format);
            }
            catch (ArgumentException)
            {
                throw new OptionException(String.Format("Unknown or unsupported binary format {0}.", format), "format");
            }
        }

        /// <summary>
        /// Sets the boot format to compile for.
        /// </summary>
        /// <param name="format">The boot format as a string.</param>
        private void SetBootFormat(string format)
        {
            switch (format.ToLower())
            {
                case "multiboot-0.7":
                case "mb0.7":
                    Multiboot0695AssemblyStage stage = new Multiboot0695AssemblyStage();
                    stage.AddOptions(optionSet);
                    bootFormatStage = stage;
                    break;
            }
        }

        /// <summary>
        /// Sets the output file for the compilation.
        /// </summary>
        /// <param name="file">The file.</param>
        private void SetOutputFile(string file)
        {
            outputFile = file;
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
        /// Adds all options to the OptionSet, to print them in the help text.
        /// </summary>
        private void AddAllOptions()
        {
            new Multiboot0695AssemblyStage().AddOptions(optionSet);
        }

        /// <summary>
        /// Parses the given string to an enumeration value.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="value">The string to parse.</param>
        /// <returns>The enumeration value.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="value"/> is not a member of the enumeration <typeparamref name="T"/></exception>
        private T EnumParse<T>(string value)
        {
            T parsed = (T) Enum.Parse(typeof(T), value, true);
            if (false == Enum.IsDefined(typeof(T), parsed))
            {
                throw new ArgumentException();
            }
            return parsed;
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
