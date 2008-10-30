using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;
using System.IO;
using Mosa.Runtime.Linker;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base class for object file builders.
    /// </summary>
    public abstract class ObjectFileBuilderBase : IAssemblyLinker
    {
        /// <summary>
        /// The name of the object file builder
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Called when an assembly's compilation begins
        /// </summary>
        /// <param name="compiler">The compiler</param>
        public virtual void OnAssemblyCompileBegin(AssemblyCompiler compiler) { }

        /// <summary>
        /// Called when an assembly's compilation ends
        /// </summary>
        /// <param name="compiler">The compiler</param>
        public virtual void OnAssemblyCompileEnd(AssemblyCompiler compiler) { }

        /// <summary>
        /// Called when an methods's compilation begins
        /// </summary>
        /// <param name="compiler">The compiler</param>
        public virtual void OnMethodCompileBegin(MethodCompilerBase compiler) { }

        /// <summary>
        /// Called when an methods's compilation ends
        /// </summary>
        /// <param name="compiler">The compiler</param>
        public virtual void OnMethodCompileEnd(MethodCompilerBase compiler) { }

        /// <summary>
        /// Allocates the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="section">The section.</param>
        /// <param name="size">The size.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns></returns>
        public virtual Stream Allocate(RuntimeMember member, SectionKind section, int size, int alignment)
        {
            return null;
        }

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        public virtual Stream Allocate(string name, SectionKind section, int size, int alignment)
        {
            return null;
        }

        /// <summary>
        /// Issues a linker request for the given runtime method.
        /// </summary>
        /// <param name="method">The method the patched code belongs to.</param>
        /// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
        /// <param name="linkType">The type of link required.</param>
        /// <param name="methodRelativeBase">The base address, if a relative link is required.</param>
        /// <param name="target">The method or static field to link against.</param>
        /// <returns>
        /// The return value is the preliminary address to place in the generated machine 
        /// code. On 32-bit systems, only the lower 32 bits are valid. The above are not used. An implementation of
        /// IAssemblyLinker may not rely on 64-bits being stored in the memory defined by position.
        /// </returns>
        public virtual long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, RuntimeMember target)
        {
            return 0;
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
        public virtual long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, string symbol)
        {
            return 0;
        }

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public ICollection<Linker.LinkerSection> Sections { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Retrieves the collection of symbols known by the linker.
        /// </summary>
        /// <value>The symbol collection.</value>
        public ICollection<LinkerSymbol> Symbols { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <value>The base address.</value>
        public long BaseAddress { get { return 0L; } }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp { get { return DateTime.Now; } }

        /// <summary>
        /// Gets the entry point symbol.
        /// </summary>
        /// <value>The entry point symbol.</value>
        public LinkerSymbol EntryPoint { get { return null; } }
    }
}
