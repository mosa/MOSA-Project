/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
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
    public class ArchitectureSelector : IHasOptions
    {
        /// <summary>
        /// Holds the real stage implementation to use.
        /// </summary>
        private IArchitecture implementation;
        
        /// <summary>
        /// Initializes a new instance of the ArchitectureSelector class.
        /// </summary>
        public ArchitectureSelector()
        {
            this.implementation = null;
        }
        
        private IArchitecture SelectImplementation(string architecture)
        {
            switch (architecture.ToLower())
            {
                case "x86":
                    return Mosa.Platforms.x86.Architecture.CreateArchitecture(Mosa.Platforms.x86.ArchitectureFeatureFlags.AutoDetect);

                case "x64":
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
                "Select one of the MOSA architectures to compile for [{x86}].",
                delegate(string arch)
                {
                    this.implementation = SelectImplementation(arch);
                }
            );
        }
        
        /// <summary>
        /// Gets the selected architecture.
        /// </summary>
        public IArchitecture Architecture
        {
            get
            {
                CheckImplementation();
                return this.implementation;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an implementation is selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if an implementation is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfigured
        {
            get { return (this.implementation != null); }
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
