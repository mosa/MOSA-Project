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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    public class LdlocaInstruction : LoadInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LdlocaInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public LdlocaInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Ldloca == code || OpCode.Ldloca_s == code);
            if (OpCode.Ldloca != code && OpCode.Ldloca_s != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// From the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            ushort locIdx;

            // Opcode specific handling 
            if (_code == OpCode.Ldloca_s)
            {
                byte loc;
                decoder.Decode(out loc);
                locIdx = loc;
            }
            else
            {
                decoder.Decode(out locIdx);
            }

            Operand paramOp = decoder.Compiler.GetLocalOperand(locIdx);
            SetOperand(0, paramOp);

            Operand result = decoder.Compiler.CreateTemporary(new RefSigType(paramOp.Type));
            SetResult(0, result);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldloca(this, arg);
        }

        #endregion // Methods
    }
}
