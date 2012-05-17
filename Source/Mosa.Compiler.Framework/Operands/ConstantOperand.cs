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
		#region Static Data

		public static ConstantOperand I4_0 = new ConstantOperand(BuiltInSigType.Int32, (int)0);
		public static ConstantOperand I4_1 = new ConstantOperand(BuiltInSigType.Int32, (int)1);
		public static ConstantOperand I4_2 = new ConstantOperand(BuiltInSigType.Int32, (int)2);
		public static ConstantOperand I4_3 = new ConstantOperand(BuiltInSigType.Int32, (int)3);
		public static ConstantOperand I4_4 = new ConstantOperand(BuiltInSigType.Int32, (int)4);
		public static ConstantOperand I4_5 = new ConstantOperand(BuiltInSigType.Int32, (int)5);
		public static ConstantOperand I4_6 = new ConstantOperand(BuiltInSigType.Int32, (int)6);
		public static ConstantOperand I4_7 = new ConstantOperand(BuiltInSigType.Int32, (int)7);
		public static ConstantOperand I4_8 = new ConstantOperand(BuiltInSigType.Int32, (int)8);
		public static ConstantOperand I4_16 = new ConstantOperand(BuiltInSigType.Int32, (int)16);
		public static ConstantOperand I4_32 = new ConstantOperand(BuiltInSigType.Int32, (int)32);
		public static ConstantOperand I4_64 = new ConstantOperand(BuiltInSigType.Int32, (int)64);
		public static ConstantOperand I4_N1 = new ConstantOperand(BuiltInSigType.Int32, (int)-1);

		public static ConstantOperand U1_0 = new ConstantOperand(BuiltInSigType.Byte, 0);
		public static ConstantOperand U1_1 = new ConstantOperand(BuiltInSigType.Byte, 1);

		public static ConstantOperand U4_0 = new ConstantOperand(BuiltInSigType.UInt32, (int)0);
		public static ConstantOperand U4_0xFFFFFFFF = new ConstantOperand(BuiltInSigType.UInt32, (uint)(0xFFFFFFFF));
		public static ConstantOperand Obj_Null = new ConstantOperand(BuiltInSigType.Object, null);

		#endregion

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
			switch (value)
			{
				case 0: return I4_0;
				case 1: return I4_1;
				case 2: return I4_2;
				case 3: return I4_3;
				case 4: return I4_4;
				case 5: return I4_5;
				case 6: return I4_6;
				case 7: return I4_7;
				case 8: return I4_8;
				case 16: return I4_16;
				case 32: return I4_32;
				case 64: return I4_64;
				case -1: return I4_N1;
				default: return new ConstantOperand(BuiltInSigType.Int32, value);
			}

		}

		/// <summary>
		/// Retrieves a constant operand to represent the null-value.
		/// </summary>
		/// <returns>A new instance of <see cref="ConstantOperand"/>, that represents the null value.</returns>
		public static ConstantOperand GetNull()
		{
			return Obj_Null;
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
				return String.Format("const null [{0}]", type);
			return String.Format("const {0} [{1}]", value, type);
		}

		#endregion // Operand Overrides
	}
}


