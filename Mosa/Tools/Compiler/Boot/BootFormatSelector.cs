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

using Mosa.Runtime.CompilerFramework;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Boot
{
    /// <summary>
    /// Selector proxy type for the boot format. 
    /// </summary>
    public class BootFormatSelector : IAssemblyCompilerStage, IHasOptions
    {
        IAssemblyCompilerStage implementation;
        
        /// <summary>
        /// Initializes a new instance of the BootFormatSelector class.
        /// Selects an appropriate implementation of the boot format stage based on the format.
        /// </summary>
        /// <param name="format">The format.</param>
        public BootFormatSelector(string format)
        {
            implementation = SelectImplementation(format);
        }
        
        private IAssemblyCompilerStage SelectImplementation(string format)
        {
            switch (format.ToLower())
            {
                case "multiboot-0.7":
                case "mb0.7":
                    return new Multiboot0695AssemblyStage();
                default:
                    throw new OptionException(String.Format("Unknown or unsupported boot format {0}.", format), "boot");
            }
        }
        
        /// <summary>
        /// Adds the additional options for the parsing process to the given OptionSet.
        /// </summary>
        /// <param name="optionSet">A given OptionSet to add the options to.</param>
        public void AddOptions(OptionSet optionSet)
        {
            ((IHasOptions)implementation).AddOptions(optionSet);
        }
        
        /// <summary>
        /// Adds the additional options for all possible implementations of this type for the parsing process to the given OptionSet.
        /// </summary>
        /// <param name="optionSet">A given OptionSet to add the options to.</param>
        public static void AddOptionsForAll(OptionSet optionSet)
        {
            new Multiboot0695AssemblyStage().AddOptions(optionSet);
        }
        
        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get
            {
                return implementation.Name;
            }
        }
        
        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            implementation.Run(compiler);
        }
    }
}
