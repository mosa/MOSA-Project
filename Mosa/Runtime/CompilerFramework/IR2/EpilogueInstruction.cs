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

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// An abstract intermediate representation of the method epilogue.
    /// </summary>
    /// <remarks>
    /// This instruction is usually derived by the architecture and expanded appropriately
    /// for the calling convention of the method.
    /// </remarks>
    public class EpilogueInstruction: BaseInstruction
    {
        #region Data members

        /// <summary>
        /// Holds the stack size requirements.
        /// </summary>
        private int _stackSize;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="EpilogueInstruction"/>.
        /// </summary>
        /// <param name="stackSize">Specifies the stack size requirements of the method.</param>
        public EpilogueInstruction(int stackSize) :
            base(0, 0)
        {
            _stackSize = stackSize;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the stack size requirements of the method.
        /// </summary>
        public int StackSize
        {
            get { return _stackSize; }
            set { _stackSize = value; }
        }

        #endregion // Properties

        #region Instruction Overrides

		/// <summary>
		/// Returns a string representation of the <see cref="EpilogueInstruction"/>.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A string representation of the instruction.
		/// </returns>
        public override string ToString(ref InstructionData instruction)
        {
            return @"IR epilogue";
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.EpilogueInstruction(context);
		}

        #endregion // Instruction Overrides
    }
}
