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
	/// Abstract base class for all instructions in the intermediate representation.
	/// </summary>
	public abstract class BaseInstruction : IIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseInstruction"/>.
		/// </summary>
		protected BaseInstruction()
			: this(0, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BaseInstruction"/>.
		/// </summary>
		/// <param name="operandCount">Specifies the number of operands of the context.</param>
		protected BaseInstruction(int operandCount) :
			this(operandCount, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BaseInstruction"/>.
		/// </summary>
		/// <param name="operandCount">Specifies the number of operands of the context.</param>
		/// <param name="resultCount">Specifies the number of results of the context.</param>
		protected BaseInstruction(int operandCount, int resultCount)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Determines flow behavior of this context.
		/// </summary>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public virtual FlowControl FlowControl
		{
			get { return FlowControl.Next; }
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public virtual string ToString(Context ctx)
		{
			return ToString();
		}

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "IR." + this.GetType().ToString();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(IIRVisitor visitor, Context context)
		{
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(IVisitor visitor, Context context)
		{
			if (visitor is IIRVisitor)
				Visit(visitor as IIRVisitor, context);
		}

		#endregion // Methods
	}
}
