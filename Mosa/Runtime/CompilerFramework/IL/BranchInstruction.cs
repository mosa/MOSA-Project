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
    public class BranchInstruction : ILInstruction, IBranchInstruction
    {
        #region Data members

        /// <summary>
        /// Stores the branch target.
        /// </summary>
        protected int[] _branchTargets;

        #endregion // Data members

        #region Construction

        public BranchInstruction(OpCode code)
            : base(code)
        {
        }

        public BranchInstruction(OpCode code, int[] branchTargets)
            : base(code)
        {
            _branchTargets = branchTargets;
        }

        #endregion // Construction

        #region Properties

        public int[] BranchTargets
        {
            get { return _branchTargets; }
            set { _branchTargets = value; }
        }

        public override FlowControl FlowControl
        {
            get { return FlowControl.Branch; }
        }

        public bool IsConditional { get { return false; } }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode bases first
            base.Decode(decoder);

            switch (_code)
            {
                case OpCode.Br_s:
                    _branchTargets = new int[] { decoder.DecodeSByte() };
                    break;

                case OpCode.Br:
                    _branchTargets = new int[] { decoder.DecodeInt32() };
                    break;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} L_{1:X4}", base.ToString(), _branchTargets[0]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Branch(this, arg);
        }

        #endregion // Methods
    }
}
