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

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Vm;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Selector proxy type for the linker. 
    /// </summary>
    public class LinkerFormatSelector : IAssemblyLinker, IAssemblyCompilerStage, IHasOptions
    {
        AssemblyLinkerStageBase implementation;
        
        /// <summary>
        /// Initializes a new instance of the LinkerFormalSelector class.
        /// Selects an appropriate implementation of the linker based on the format.
        /// </summary>
        /// <param name="format">The format.</param>
        public LinkerFormatSelector(string format)
        {
            implementation = SelectImplementation(format);
        }
        
        private AssemblyLinkerStageBase SelectImplementation(string format)
        {
            switch (format.ToLower())
            {
                case "elf":
                    throw new NotImplementedException("ELF linker stage is not implemented yet.");
                case "pe":
                    throw new NotImplementedException("PE linker stage is not implemented yet.");
                default:
                    throw new OptionException(String.Format("Unknown or unsupported binary format {0}.", format), "format");
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
        /// Gets the base address.
        /// </summary>
        /// <value>The base address.</value>
        public long BaseAddress
        {
            get
            {
                return implementation.BaseAddress;
            }
        }

        /// <summary>
        /// Gets the entry point symbol.
        /// </summary>
        /// <value>The entry point symbol.</value>
        public LinkerSymbol EntryPoint
        {
            get
            {
                return implementation.EntryPoint;
            }
        }

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public ICollection<LinkerSection> Sections
        {
            get
            {
                return implementation.Sections;
            }
        }

        /// <summary>
        /// Retrieves the collection of symbols known by the linker.
        /// </summary>
        /// <value>The symbol collection.</value>
        public ICollection<LinkerSymbol> Symbols
        {
            get
            {
                return implementation.Symbols;
            }
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp
        {
            get
            {
                return implementation.TimeStamp;
            }
        }

        /// <summary>
        /// Issues a linker request for the given runtime method.
        /// </summary>
        /// <param name="linkType">The type of link required.</param>
        /// <param name="method">The method the patched code belongs to.</param>
        /// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
        /// <param name="methodRelativeBase">The base address, if a relative link is required.</param>
        /// <param name="target">The method or static field to link against.</param>
        /// <returns>
        /// The return value is the preliminary address to place in the generated machine 
        /// code. On 32-bit systems, only the lower 32 bits are valid. The above are not used. An implementation of
        /// IAssemblyLinker may not rely on 64-bits being stored in the memory defined by position.
        /// </returns>
        public long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, RuntimeMember target)
        {
            return implementation.Link(linkType, method, methodOffset, methodRelativeBase, target);
        }

        /// <summary>
        /// Issues a linker request for the given runtime method.
        /// </summary>
        /// <param name="linkType">The type of link required.</param>
        /// <param name="method">The method the patched code belongs to.</param>
        /// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
        /// <param name="methodRelativeBase">The base address, if a relative link is required.</param>
        /// <param name="symbol">The linker symbol to link against.</param>
        /// <returns>
        /// The return value is the preliminary address to place in the generated machine 
        /// code. On 32-bit systems, only the lower 32 bits are valid. The above are not used. An implementation of
        /// IAssemblyLinker may not rely on 64-bits being stored in the memory defined by position.
        /// </returns>
        public long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, string symbol)
        {
            return implementation.Link(linkType, method, methodOffset, methodRelativeBase, symbol);
        }

        /// <summary>
        /// Allocates memory in the specified section.
        /// </summary>
        /// <param name="symbol">The metadata member to allocate space for.</param>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>A stream, which can be used to populate the section.</returns>
        public Stream Allocate(RuntimeMember symbol, SectionKind section, int size, int alignment)
        {
            return implementation.Allocate(symbol, section, size, alignment);
        }

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>A stream, which can be used to populate the section.</returns>
        public Stream Allocate(string name, SectionKind section, int size, int alignment)
        {
            return implementation.Allocate(name, section, size, alignment);
        }
    }
}
