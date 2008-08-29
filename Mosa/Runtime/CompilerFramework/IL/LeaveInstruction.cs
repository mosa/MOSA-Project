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

            _branchTargets = new int[0];
            switch (_code)
            {
                case OpCode.Leave_s:
                    {
                        sbyte sb;
                        decoder.Decode(out sb);
                        _branchTargets[0] = sb;
                    }
                    break;

                case OpCode.Leave:
                    decoder.Decode(out _branchTargets[0]);
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
