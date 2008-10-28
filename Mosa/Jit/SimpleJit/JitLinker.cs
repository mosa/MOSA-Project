/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework.Linker;
using System.IO;
using Mosa.Runtime.Vm;

namespace Mosa.Jit.SimpleJit
{
    /// <summary>
    /// A fast runtime linker.
    /// </summary>
    public sealed class JitLinker : AssemblyLinkerStageBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="JitLinker"/> class.
        /// </summary>
        public JitLinker()
        {
        }

        #endregion // Construction

        #region AssemblyLinkerStageBase Overrides

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<LinkerSection> Sections
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        protected override Stream Allocate(SectionKind section, int size, int alignment)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if the given runtime member can be resolved immediately.
        /// </summary>
        /// <param name="member">The runtime member to determine resolution of.</param>
        /// <param name="address">Receives the determined address of the runtime member.</param>
        /// <returns>
        /// The method returns true, when it was successfully resolved.
        /// </returns>
        protected override bool IsResolved(RuntimeMember member, out long address)
        {
            return base.IsResolved(member, out address);
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
            return base.Link(linkType, method, methodOffset, methodRelativeBase, target);
        }

        #endregion // AssemblyLinkerStageBase Overrides
    }
}
