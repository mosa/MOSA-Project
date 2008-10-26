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
    
    /// <summary>
    /// Available boot formats.
    /// </summary>
    public enum BootFormat
    {
        /// <summary>
        /// Boot format not defined.
        /// </summary>
        Undefined,
        
        /// <summary>
        /// The multiboot 0.7 format.
        /// </summary>
        Multiboot0_7
    }
    #endregion
    
    /// <summary>
    /// Class containing options for the compilation.
    /// </summary>
    public class CompileOptions
    {
        #region Fields

        TargetArchitecture architecture;
        BinaryFormat binaryFormat;
        BootFormat bootFormat;
        List<FileInfo> inputFiles = null;
        bool isExecutable;
        string outputFile;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CompileOptions class.
        /// </summary>
        public CompileOptions()
        {
            architecture = TargetArchitecture.Undefined;
            binaryFormat = BinaryFormat.Undefined;
            bootFormat = BootFormat.Undefined;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the architecture.
        /// </summary>
        public TargetArchitecture Architecture
        {
            get
            {
                return architecture;
            }
        }

        /// <summary>
        /// Gets the binary format.
        /// </summary>
        public BinaryFormat BinaryFormat
        {
            get
            {
                return binaryFormat;
            }
        }

        /// <summary>
        /// Gets the boot format.
        /// </summary>
        public BootFormat BootFormat
        {
            get
            {
                return bootFormat;
            }
        }

        /// <summary>
        /// Gets a list of input files.
        /// </summary>
        public IList<FileInfo> InputFiles
        {
            get
            {
                return inputFiles;
            }
        }
        
        /// <summary>
        /// Gets a list of input file names.
        /// </summary>
        public IEnumerable<string> InputFileNames
        {
            get
            {
                foreach (FileInfo file in inputFiles)
                {
                    yield return file.FullName;  
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the result of the
        /// compilation will be an executable file or not.
        /// </summary>
        public bool IsExecutable
        {
            get
            {
                return isExecutable;
            }
        }

        /// <summary>
        /// Gets the output file.
        /// </summary>
        public string OutputFile
        {
            get
            {
                return outputFile;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Sets the architecture to compile for.
        /// </summary>
        /// <param name="arch">The architecture as a string.</param>
        public void SetArchitecture(string arch)
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
        public void SetBinaryFormat(string format)
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
        public void SetBootFormat(string format)
        {
            switch (format.ToLower())
            {
                case "multiboot-0.7":
                case "mb0.7":
                    bootFormat = BootFormat.Multiboot0_7;
                    break;
            }
        }
        
        /// <summary>
        /// Finishs parsing the options and sets the list of input files.
        /// </summary>
        /// <param name="files"></param>
        public void Finish(IList<string> files)
        {            
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
                if (bootFormat == BootFormat.Undefined)
                {
                    throw new OptionException("No boot format specified or boot format unknown or not supported.", "boot");
                }
            }
            else if (bootFormat != BootFormat.Undefined)
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

        /// <summary>
        /// Sets the output file for the compilation.
        /// </summary>
        /// <param name="file">The file.</param>
        public void SetOutputFile(string file)
        {
            outputFile = file;
        }

        /// <summary>
        /// Returns a string containing the current options.
        /// </summary>
        /// <returns>A string containing the options.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Output file: ").AppendLine(outputFile);
            sb.Append("Input file(s): ").AppendLine(String.Join(", ", new List<string>(InputFileNames).ToArray()));
            sb.Append("Architecture: ").AppendLine(architecture.ToString());
            sb.Append("Binary format: ").AppendLine(binaryFormat.ToString());
            sb.Append("Boot format: ").AppendLine(bootFormat.ToString());
            sb.Append("Is executable: ").AppendLine(isExecutable.ToString());
            return sb.ToString();
        }
        #endregion Public Methods
        #region Private Methods

        private T EnumParse<T>(string value)
        {
            T parsed = (T) Enum.Parse(typeof(T), value, true);
            if (false == Enum.IsDefined(typeof(T), parsed))
            {
                throw new ArgumentException();
            }
            return parsed;
        }
        #endregion Private Methods

        #endregion Methods
    }
}
