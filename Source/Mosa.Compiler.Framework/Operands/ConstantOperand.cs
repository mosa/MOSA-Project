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
	public sealed class ConstantOperand : Operand
	{

		#region Data members

		/// <summary>
		/// Constant value.
		/// </summary>
		private object value;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantOperand"/> class.
		/// </summary>
		/// <param name="typeRef">The type ref.</param>
		/// <param name="value">The value of the constant.</param>
		public ConstantOperand(SigType typeRef, object value)
			: base(typeRef)
		{
			this.value = value;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the value of the constant.
		/// </summary>
		public object Value
		{
			get { return value; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Creates a new <see cref="ConstantOperand"/> for the given integral value.
		/// </summary>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>A new ConstantOperand representing the value <paramref name="value"/>.</returns>
		public static ConstantOperand FromValue(int value)
		{
			return new ConstantOperand(BuiltInSigType.Int32, value);
		}

		/// <summary>
		/// Retrieves a constant operand to represent the null-value.
		/// </summary>
		/// <returns>A new instance of <see cref="ConstantOperand"/>, that represents the null value.</returns>
		public static ConstantOperand GetNull()
		{
			return new ConstantOperand(BuiltInSigType.Object, null);
		}

		#endregion // Methods

		#region Operand Overrides

		/// <summary>
		/// Compares with the given operand for equality.
		/// </summary>
		/// <param name="other">The other operand to compare with.</param>
		/// <returns>The return value is true if the operands are equal; false if not.</returns>
		public override bool Equals(Operand other)
		{
			ConstantOperand cop = other as ConstantOperand;
			return (null != cop && null != cop.Value && null != Value && cop.Value.Equals(Value));
		}

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			if (value == null)
				return String.Format("const null [{0}]", _type);
			return String.Format("const {0} [{1}]", value, _type);
		}

		#endregion // Operand Overrides
	}
}


