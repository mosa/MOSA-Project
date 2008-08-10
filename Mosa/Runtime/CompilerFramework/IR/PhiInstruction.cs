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

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Used in the single static assignment form of the instruction stream to
    /// automatically select the appropriate value of a variable depending on the
    /// incoming edge.
    /// </summary>
    public sealed class PhiInstruction : Instruction
    {
        #region Data members

        /// <summary>
        /// Holds the blocks (incoming edges) of a value.
        /// </summary>
        private List<BasicBlock> _blocks = new List<BasicBlock>();

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of PhiInstruction.
        /// </summary>
        /// <param name="op">The result operand of the PhiInstruction.</param>
        public PhiInstruction(StackOperand op) :
            base(255, 1)
        {
            SetResult(0, op);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the result operand of the phi instruction.
        /// </summary>
        public StackOperand Result
        {
            get { return (StackOperand)this.Results[0]; }
        }

        /// <summary>
        /// Removes the list of blocks, that corresponds to incoming edges of the operands.
        /// </summary>
        public List<BasicBlock> Blocks
        {
            get { return _blocks; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Adds a new edge/value pair to the instruction.
        /// </summary>
        /// <param name="edge">The block, that represents the incoming edge in the control flow graph.</param>
        /// <param name="op">The stack operand, that is received on this edge.</param>
        public void AddValue(BasicBlock edge, StackOperand op)
        {
            int index = _blocks.Count;
            Debug.Assert(index < 255, @"Maximum number of operands in PHI exceeded.");
            _blocks.Add(edge);
            SetOperand(index, op);
        }

        /// <summary>
        /// Determines if the given stack operand is contained in the instruction.
        /// </summary>
        /// <param name="op">The operand to test.</param>
        /// <returns>True if the given operand is in the instruction.</returns>
        public bool Contains(StackOperand op)
        {
            return (-1 != System.Array.IndexOf(this.Operands, op));
        }

        #endregion // Methods

        #region Instruction Overrides

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("IR phi ; {0} = phi(", this.Results);
            foreach (Operand op in this.Operands)
            {
                if (null != op)
                {
                    builder.AppendFormat("{0}, ", op);
                }
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append(')');
            return builder.ToString();
        }

        public override void Visit(IInstructionVisitor visitor)
        {
            throw new NotImplementedException();
        }

        #endregion // Instruction Overrides
    }
}
