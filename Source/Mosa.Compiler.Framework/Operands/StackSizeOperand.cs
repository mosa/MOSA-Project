/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Operands
{

	/// <summary>
	/// Represent a constant operand.
	/// </summary>
	public sealed class StackSizeOperand : Operand
	{

		#region Data members

		/// <summary>
		/// Constant value.
		/// </summary>
		private StackLayout stackLayout;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StackSizeOperand"/> class.
		/// </summary>
		/// <param name="stackLayout">The stack layout.</param>
		public StackSizeOperand(StackLayout stackLayout)
			: base(BuiltInSigType.Int32)
		{
			this.stackLayout = stackLayout;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the value of the constant.
		/// </summary>
		public int Value
		{
			get { return stackLayout.StackSize; }
		}

		#endregion // Properties

		#region Operand Overrides

		/// <summary>
		/// Compares with the given operand for equality.
		/// </summary>
		/// <param name="other">The other operand to compare with.</param>
		/// <returns>The return value is true if the operands are equal; false if not.</returns>
		public override bool Equals(Operand other)
		{
			return (other is StackSizeOperand);
		}

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return String.Format("stacksize {0}", stackLayout.StackSize);
		}

		#endregion // Operand Overrides
	}
}


