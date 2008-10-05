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
    /// <summary>
    /// 
    /// </summary>
    public class LeaveInstruction : BranchInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public LeaveInstruction(OpCode code)
            : base(code)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Decodes the specified decoder.
        /// </summary>
        /// <param name="decoder">The decoder.</param>
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


        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("leave L_{0:X4}", _branchTargets);
        }

        #endregion // Methods
    }
}
