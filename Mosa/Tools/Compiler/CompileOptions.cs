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
    /// <summary>
    /// Available architectures.
    /// </summary>
    public enum Architectures
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
    public enum BinaryFormats
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
        PE }
    
    /// <summary>
    /// Available boot formats. Use '_' instead of '.'.
    /// </summary>
    public enum BootFormats
    {
        /// <summary>
        /// Boot format not defined.
        /// </summary>
        Undefined,
        
        /// <summary>
        /// The multiboot 0.7 format.
        /// </summary>
        mb0_7
    }
    
    /// <summary>
    /// Class containing options for the compilation.
    /// </summary>
    public class CompileOptions
    {
        List<string> inputFiles = null;
        string outputFile;
        Architectures architecture;
        BinaryFormats binaryFormat;
        BootFormats bootFormat;
        bool isExecutable;
        
        /// <summary>
        /// Initializes a new instance of the CompileOptions class.
        /// </summary>
        public CompileOptions()
        {
        }
        
        /// <summary>
        /// Sets the input files for the compiler.
        /// </summary>
        /// <param name="files">An enumeration of files.</param>
        /// <returns>A string containing an error message or an empty string if successful.</returns>
        public void SetInputFiles(IList<string> files)
        {
            inputFiles = new List<string>(files);
            
            if (inputFiles.Count == 0)
            {
                throw new OptionException("No input file(s) specified.", "");
            }
            
            foreach (string file in inputFiles)
            {
                string extension = new FileInfo(file).Extension;
                if (extension.ToLower() == ".exe")
                {
                    if (isExecutable)
                    {
                        // there are more than one exe files in the list
                        throw new OptionException("Multiple executables aren't allowed.", "");
                    }
                    
                    isExecutable = true;
                }
            }
        }
        
        /// <summary>
        /// Sets the output file for the compilation.
        /// </summary>
        /// <param name="file">The file.</param>
        public void SetOutputFile(string file)
        {
            if (String.IsNullOrEmpty(file))
            {
                throw new OptionException("No output file specified.", "o");
            }
            
            outputFile = file;
        }
        
        /// <summary>
        /// Sets the architecture to compile for.
        /// </summary>
        /// <param name="arch">The architecture as a string.</param>
        public void SetArchitecture(string arch)
        {
            if (String.IsNullOrEmpty(arch))
            {
                throw new OptionException("No architecture specified.", "arch");
            }
            
            try
            {
                architecture = EnumParse<Architectures>(arch);
            }
            catch(ArgumentException)
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
            if (String.IsNullOrEmpty(format))
            {
                throw new OptionException("No binary format specified.", "format");
            }
            
            try
            {
                binaryFormat = EnumParse<BinaryFormats>(format);
            }
            catch(ArgumentException)
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
            // boot format only matters if it's an executable
            if (isExecutable)
            {
                if (String.IsNullOrEmpty(format))
                {
                    throw new OptionException("No boot format specified.", "boot");
                }
                
                try
                {
                    bootFormat = EnumParse<BootFormats>(format.Replace('.', '_'));
                }
                catch(ArgumentException)
                {
                    throw new OptionException(String.Format("Unknown or unsupported boot format {0}.", format), "boot");
                }
            }
        }
        
        /// <summary>
        /// Gets a list of input files.
        /// </summary>
        public IList<string> InputFiles
        {
            get
            {
                return inputFiles;
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
        
        /// <summary>
        /// Gets the architecture.
        /// </summary>
        public Architectures Architecture
        {
            get
            {
                return architecture;
            }
        }
        
        /// <summary>
        /// Gets the binary format.
        /// </summary>
        public BinaryFormats BinaryFormat
        {
            get
            {
                return binaryFormat;
            }
        }
        
        /// <summary>
        /// Gets the boot format.
        /// </summary>
        public BootFormats BootFormat
        {
            get
            {
                return bootFormat;
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
        /// Returns a string containing the current options.
        /// </summary>
        /// <returns>A string containing the options.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Output file: ").AppendLine(outputFile);
            sb.Append("Input file(s): ").AppendLine(String.Join(", ", inputFiles.ToArray()));
            sb.Append("Architecture: ").AppendLine(architecture.ToString());
            sb.Append("Binary format: ").AppendLine(binaryFormat.ToString());
            sb.Append("Boot format: ").AppendLine(bootFormat.ToString());
            sb.Append("Is executable: ").AppendLine(isExecutable.ToString());
            return sb.ToString();
        }
        
        private T EnumParse<T>(string value)
        {
            T parsed = (T) Enum.Parse(typeof(T), value, true);
            if (false == Enum.IsDefined(typeof(T), parsed))
            {
                throw new ArgumentException();
            }
            return parsed;
        }
    }
}
