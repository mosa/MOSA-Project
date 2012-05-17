/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for all instructions in the intermediate representation.
	/// </summary>
	public abstract class BaseIRInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseIRInstruction"/>.
		/// </summary>
		protected BaseIRInstruction()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BaseIRInstruction"/>.
		/// </summary>
		/// <param name="operandCount">Specifies the number of operands of the context.</param>
		protected BaseIRInstruction(byte operandCount) :
			base(operandCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BaseIRInstruction"/>.
		/// </summary>
		/// <param name="operandCount">Specifies the number of operands of the context.</param>
		/// <param name="resultCount">Specifies the number of results of the context.</param>
		protected BaseIRInstruction(byte operandCount, byte resultCount)
			: base(operandCount, resultCount)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "IR." + base.ToString();
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
		public override void Visit(IVisitor visitor, Context context)
		{
			if (visitor is IIRVisitor)
				Visit(visitor as IIRVisitor, context);
		}

		#endregion // Methods
	}
}
