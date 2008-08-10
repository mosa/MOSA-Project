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

using Mosa.Runtime.Metadata;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Represents a basic jump instruction.
    /// </summary>
    /// <remarks>
    /// Other more complex method invocation instructions derive from this class, specifically the CallInstruction,
    /// the CalliInstruction and CallvirtInstruction classes. They share the features provided by the JumpInstruction.
    /// </remarks>
    public class JumpInstruction : InvokeInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="JumpInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the instruction.</param>
        public JumpInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Jmp == code);
        }

        #endregion // Construction

        #region Properties

        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef; }
        }

        #endregion // Properties

        #region Methods

        protected void ProcessStack(IInstructionDecoder decoder)
        {
            // ISO/IEC 23271:2006(E), Partition III, §3.37:
            // A jump instruction may not have arguments on the stack, the stack must be empty.
            // FIXME: if (0 != decoder.OperandStack.Count)
            // FIXME:     throw new InvalidOperationException(@"IL operand stack is not empty.");
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Jmp(this);
        }

        #endregion // Methods
    }
}
