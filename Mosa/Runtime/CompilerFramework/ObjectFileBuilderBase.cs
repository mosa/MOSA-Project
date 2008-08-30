using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
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
    }
}
