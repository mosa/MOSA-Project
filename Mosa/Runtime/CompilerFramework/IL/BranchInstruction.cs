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
    /// <summary>
    /// 
    /// </summary>
    public class BranchInstruction : ILInstruction, IBranchInstruction
    {
        #region Data members

        /// <summary>
        /// Stores the branch target.
        /// </summary>
        protected int[] _branchTargets;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public BranchInstruction(OpCode code)
            : base(code)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="branchTargets">The branch targets.</param>
        public BranchInstruction(OpCode code, int[] branchTargets)
            : base(code)
        {
            _branchTargets = branchTargets;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Returns the branch targets instruction index.
        /// </summary>
        /// <value></value>
        public int[] BranchTargets
        {
            get { return _branchTargets; }
            set { _branchTargets = value; }
        }

        /// <summary>
        /// Determines flow behavior of this instruction.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Knowledge of control flow is required for correct basic block
        /// building. Any instruction that alters the control flow must override
        /// this property and correctly identify its control flow modifications.
        /// </remarks>
        public override FlowControl FlowControl
        {
            get { return FlowControl.Branch; }
        }

        /// <summary>
        /// Determines if the branch is conditional.
        /// </summary>
        /// <value></value>
        public bool IsConditional { get { return false; } }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode bases first
            base.Decode(decoder);

            _branchTargets = new int[1];
            switch (_code)
            {
                case OpCode.Br_s:
                    {
                        sbyte target;
                        decoder.Decode(out target);
                        _branchTargets[0] = target;
                    }
                    break;

                case OpCode.Br:
                    decoder.Decode(out _branchTargets[0]);
                    break;
            }
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
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
