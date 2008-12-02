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
        /// <summary>
        /// Holds the real stage implementation to use.
        /// </summary>
        private IAssemblyCompilerStage implementation;

        /// <summary>
        /// Holds the Multiboot 0.7 stage.
        /// </summary>
        private IAssemblyCompilerStage multiboot07Stage;
        
        /// <summary>
        /// Initializes a new instance of the BootFormatSelector class.
        /// </summary>
        public BootFormatSelector()
        {
            this.multiboot07Stage = new Multiboot0695AssemblyStage();
            this.implementation = null;
        }
        
        private IAssemblyCompilerStage SelectImplementation(string format)
        {
            switch (format.ToLower())
            {
                case "multiboot-0.7":
                case "mb0.7":
                    return this.multiboot07Stage;
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
            IHasOptions options;
            
            optionSet.Add(
                "b|boot=",
                "Specify the bootable format of the produced binary [{mb0.7}].",
                delegate(string format)
                {
                    this.implementation = SelectImplementation(format);
                }
            );
            
            options = multiboot07Stage as IHasOptions;
            if (options != null)
                options.AddOptions(optionSet);
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
                throw new InvalidOperationException(@"BootFormatSelector not initialized.");
        }
    }
}
