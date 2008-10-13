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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation for various IL call operations.
    /// </summary>
    /// <remarks>
    /// Instances of this class are used to represent call, calli and callvirt
    /// instructions.
    /// </remarks>
    public class CallInstruction : InvokeInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CallInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the invoke instruction.</param>
        public CallInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Call == code);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the supported immediate metadata tokens in the instruction.
        /// </summary>
        /// <value></value>
        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Call(this, arg);
        }

        #endregion // Methods
    }
}
