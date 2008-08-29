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

        public LdargaInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Ldarga == code || OpCode.Ldarga_s == code);
            if (OpCode.Ldarga != code && OpCode.Ldarga_s != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

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
            
            // Create a new operand based on the given one...
            // FIXME: Operand looses memory location information!!
            SetResult(0, decoder.Compiler.CreateResultOperand(new RefSigType(decoder.GetParameterOperand(argIdx).Type)));
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
