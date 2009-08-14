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
    /// Intermediate representation of the IL ldarga and ldarga.s instructions.
    /// </summary>
    public class LdargaInstruction : LoadInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LdargaInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public LdargaInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Ldarga == code || OpCode.Ldarga_s == code);
            if (OpCode.Ldarga != code && OpCode.Ldarga_s != code)
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
            ushort argIdx;

            // Opcode specific handling 
            if (_code == OpCode.Ldarga_s)
            {
                byte arg;
                decoder.Decode(out arg);
                argIdx = arg;
            }
            else
            {
                decoder.Decode(out argIdx);
            }

            Operand paramOp = decoder.Compiler.GetParameterOperand(argIdx);
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
            visitor.Ldarga(this, arg);
        }

        #endregion // Methods
    }
}
