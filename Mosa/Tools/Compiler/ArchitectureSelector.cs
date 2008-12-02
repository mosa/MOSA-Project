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

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Selector proxy type for the architecture. 
    /// </summary>
    public class ArchitectureSelector : IAssemblyCompilerStage, IHasOptions
    {
        IAssemblyCompilerStage implementation;
        
        /// <summary>
        /// Initializes a new instance of the ArchitectureSelector class.
        /// Selects an appropriate implementation of the architecture stage
        /// based on the architecture's name.
        /// </summary>
        /// <param name="architecture">The architecture.</param>
        public ArchitectureSelector(string architecture)
        {
            implementation = SelectImplementation(architecture);
        }
        
        private IAssemblyCompilerStage SelectImplementation(string architecture)
        {
            switch (architecture.ToLower())
            {
                case "x86":
                    throw new NotImplementedException("x86 architecture not implemented yet.");
                case "x64":
                    throw new NotImplementedException("x64 architecture not implemented yet.");
                default:
                    throw new OptionException(String.Format("Unknown or unsupported architecture {0}.", architecture), "arch");
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
            throw new NotImplementedException();
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
