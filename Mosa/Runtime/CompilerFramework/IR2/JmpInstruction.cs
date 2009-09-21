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
    /// Intermediate representation of an unconditional branch instruction.
    /// </summary>
    public class JmpInstruction : IRInstruction
    {
        #region Data members

        /// <summary>
        /// The jump destination.
        /// </summary>
        private int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="JmpInstruction"/> class.
        /// </summary>
        /// <param name="label">The jump destination label.</param>
        public JmpInstruction(int label)
        {
            _label = label;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the jump destination label.
        /// </summary>
        /// <value>The label.</value>
        public int Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #endregion // Properties

        #region IRInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(ref InstructionData instruction)
        {
            return String.Format(@"IR jmp {0}", _label);
        }

		/// <summary>
		/// Visits the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.JmpInstruction(context);
        }

        #endregion // IRInstruction Overrides
    }
}
