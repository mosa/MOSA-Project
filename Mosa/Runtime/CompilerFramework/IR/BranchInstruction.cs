/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Intermediate representation of a branch context.
	/// </summary>
	public sealed class BranchInstruction : OneOperandInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the condition code to check in the branch.
		/// </summary>
		private ConditionCode _conditionCode;

		/// <summary>
		/// Holds the branch target label.
		/// </summary>
		private int _label;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BranchInstruction"/> class.
		/// </summary>
		public BranchInstruction()
		{
			_label = 0;
			_conditionCode = ConditionCode.Equal;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BranchInstruction"/> class.
		/// </summary>
		/// <param name="cc">The condition code to branch upon.</param>
		/// <param name="label">The destination label.</param>
		public BranchInstruction(ConditionCode cc, int label)
		{
			_conditionCode = cc;
			_label = label;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		public ConditionCode ConditionCode
		{
			get { return _conditionCode; }
			set { _conditionCode = value; }
		}

		#endregion // Properties

		#region OneOperandInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.BranchInstruction(context);
		}

		#endregion // OneOperandInstruction Overrides

	}
}
