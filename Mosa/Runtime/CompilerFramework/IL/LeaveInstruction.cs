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
    // FIXME: Clears the stack
    public class LeaveInstruction : BranchInstruction
    {
        #region Construction

        public LeaveInstruction(OpCode code)
            : base(code)
        {
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode bases first
            base.Decode(decoder);

            switch (_code)
            {
                case OpCode.Leave_s:
                    _branchTargets = new int[] { decoder.DecodeSByte() };
                    break;

                case OpCode.Leave:
                    _branchTargets = new int[] { decoder.DecodeInt32() };
                    break;
            }
        }


        public override string ToString()
        {
            return String.Format("leave L_{0:X4}", _branchTargets);
        }

        #endregion // Methods
    }
}
