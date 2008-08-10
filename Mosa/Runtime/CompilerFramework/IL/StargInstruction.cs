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

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class StargInstruction : UnaryInstruction, IStoreInstruction
    {
        #region Construction

        public StargInstruction(OpCode code)
            : base(code, 1)
        {
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            ushort argIdx;

            // Decode the base first
            base.Decode(decoder);

            // Opcode specific handling 
            if (_code == OpCode.Starg_s)
                argIdx = decoder.DecodeByte();
            else
                argIdx = decoder.DecodeUInt16();

            // The argument is the result
            this.SetResult(0, decoder.GetParameterOperand(argIdx));

            // FIXME: Do some type compatibility checks
            // See verification for this instruction and
            // verification types.
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Starg(this);
        }

        #endregion // Methods
    }
}
