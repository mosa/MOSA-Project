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
using Mosa.Runtime.CompilerFramework.IL;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class InitblkInstruction : NaryInstruction
    {
        #region Construction

        public InitblkInstruction(OpCode code)
            : base(code, 3)
        {
            Debug.Assert(OpCode.Initblk == code);
            if (OpCode.Initblk != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base
            base.Decode(decoder);

            // FIXME: Validate & verify
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Initblk(this, arg);
        }

        #endregion // Methods
    }
}
