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
using IR2 = Mosa.Runtime.CompilerFramework.IR2;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 cmp instruction.
    /// </summary>
    public sealed class CmpInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CmpInstruction"/> class.
        /// </summary>
        public CmpInstruction()
        {
        }

        #endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

        #region Methods

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86 cmp {0}, {1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cmp(context);
		}

        #endregion // Methods
    }
}
