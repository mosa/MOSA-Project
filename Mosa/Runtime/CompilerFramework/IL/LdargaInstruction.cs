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
                argIdx = decoder.DecodeByte();
            else
                argIdx = decoder.DecodeUInt16();
            
            // Create a new operand based on the given one...
            // FIXME: Operand looses memory location information!!
            SetResult(0, CreateResultOperand(decoder.Architecture, new Ref(decoder.GetParameterOperand(argIdx).Type)));
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ldarga(this);
        }

        #endregion // Methods
    }
}
