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
        /// <summary>
        /// Holds the real stage implementation to use.
        /// </summary>
        private IAssemblyCompilerStage implementation;
        
        /// <summary>
        /// Initializes a new instance of the ArchitectureSelector class.
        /// </summary>
        public ArchitectureSelector()
        {
            this.implementation = null;
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
            optionSet.Add(
                "a|arch=",
                "Select one of the MOSA architectures to compile for [{x86|x64}].",
                delegate(string arch)
                {
                    this.implementation = SelectImplementation(arch);
                }
            );
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
        /// Gets a value indicating wheter an implementation has been selected.
        /// </summary>
        public bool IsImplementationSelected
        {
            get
            {
                return (implementation != null);
            }
        }
        
        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get
            {
                if (implementation == null)
                    return @"Boot Format Selector";
                return implementation.Name;
            }
        }
        
        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            CheckImplementation();
            implementation.Run(compiler);
        }
        
        /// <summary>
        /// Checks if an implementation is set.
        /// </summary>
        private void CheckImplementation()
        {
            if (this.implementation == null)
                throw new InvalidOperationException(@"ArchitectureFormatSelector not initialized.");
        }
    }
}
