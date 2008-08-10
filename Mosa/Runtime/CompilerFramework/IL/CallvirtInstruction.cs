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
using System.Diagnostics;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Represents a virtual method call instruction.
    /// </summary>
    public class CallvirtInstruction : InvokeInstruction
    {
        #region Construction

        /// <summary>
        /// Decode a new instance of <see cref="CallvirtInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the CallvirtInstruction.</param>
        public CallvirtInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Callvirt == code);
        }

        #endregion // Construction

        #region Properties

        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
        }

        #endregion // Properties

        #region Methods

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Callvirt(this);
        }

        #endregion // Methods
    }
}
