/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using System.IO;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class AotLinkerStage : AssemblyLinkerStageBase
    {
        /// <summary>
        /// 
        /// </summary>
        ObjectFileBuilderBase _objectFileBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectFileBuilder"></param>
        public AotLinkerStage(ObjectFileBuilderBase objectFileBuilder)
        {
            this._objectFileBuilder = objectFileBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return "AOT Linker"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compiler"></param>
        public void Run(AssemblyCompiler compiler) { }

        #region IAssemblyLinker Members

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        protected override Stream Allocate(LinkerSection section, int size, int alignment)
        {
            return _objectFileBuilder.Allocate((string)null, section, size, alignment);
        }

        /// <summary>
        /// Issues a linker request for the given runtime method.
        /// </summary>
        /// <param name="linkType">The type of link required.</param>
        /// <param name="method">The method the patched code belongs to.</param>
        /// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
        /// <param name="methodRelativeBase">The base address, if a relative link is required.</param>
        /// <param name="target">The method or static field to link against.</param>
        /// <returns></returns>
        public override long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, RuntimeMember target)
        {
            return _objectFileBuilder.Link(
                linkType,
                method,
                methodOffset,
                methodRelativeBase,
                target
            );
        }

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="method">The method whose code is being patched.</param>
        /// <param name="methodOffset">The value to store at the position in code.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="targetAddress">The position in code, where it should be patched.</param>
        protected override void ApplyPatch(LinkType linkType, RuntimeMethod method, long methodOffset, long methodRelativeBase, long targetAddress)
        {
        }

        #endregion
    }
}
