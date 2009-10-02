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
using Mosa.Runtime.CompilerFramework;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of a branch instruction.
    /// </summary>
    public sealed class JnsBranchInstruction : OneOperandInstruction
    {
        #region Data members
        /// <summary>
        /// Holds the branch target label.
        /// </summary>
        private int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="JnsBranchInstruction"/> class.
        /// </summary>
        public JnsBranchInstruction()
        {
            _label = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JnsBranchInstruction"/> class.
        /// </summary>
        /// <param name="label">The destination label.</param>
        public JnsBranchInstruction(int label)
        {
            _label = label;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public int Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #endregion // Properties

        #region OneOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR br.>= 0 {0} ; if >= 0 goto {0}", _label);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Jns(context);
		}

        #endregion // OneOperandInstruction Overrides

    }
}
