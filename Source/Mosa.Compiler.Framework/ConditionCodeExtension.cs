// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Condition Code Extension
	/// </summary>
	public static class ConditionCodeExtension
	{
		#region Utility Methods

		/// <summary>
		/// Gets the unsigned condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code to get an unsigned form from.</param>
		/// <returns>The unsigned form of the given condition code.</returns>
		public static ConditionCode GetUnsigned(this ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: break;
				case ConditionCode.NotEqual: break;
				case ConditionCode.GreaterOrEqual: return ConditionCode.UnsignedGreaterOrEqual;
				case ConditionCode.GreaterThan: return ConditionCode.UnsignedGreaterThan;
				case ConditionCode.LessOrEqual: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.LessThan: return ConditionCode.UnsignedLessThan;
				case ConditionCode.UnsignedGreaterOrEqual: break;
				case ConditionCode.UnsignedGreaterThan: break;
				case ConditionCode.UnsignedLessOrEqual: break;
				case ConditionCode.UnsignedLessThan: break;
				default: throw new NotSupportedException();
			}

			return conditionCode;
		}

		/// <summary>
		/// Gets the opposite condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code.</param>
		/// <returns></returns>
		/// <exception cref="System.NotSupportedException"></exception>
		public static ConditionCode GetOpposite(this ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return ConditionCode.NotEqual;
				case ConditionCode.NotEqual: return ConditionCode.Equal;
				case ConditionCode.GreaterOrEqual: return ConditionCode.LessThan;
				case ConditionCode.GreaterThan: return ConditionCode.LessOrEqual;
				case ConditionCode.LessOrEqual: return ConditionCode.GreaterThan;
				case ConditionCode.LessThan: return ConditionCode.GreaterOrEqual;
				case ConditionCode.UnsignedGreaterOrEqual: return ConditionCode.UnsignedLessThan;
				case ConditionCode.UnsignedGreaterThan: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.UnsignedLessOrEqual: return ConditionCode.UnsignedGreaterThan;
				case ConditionCode.UnsignedLessThan: return ConditionCode.UnsignedGreaterOrEqual;
				case ConditionCode.Signed: return ConditionCode.NotSigned;
				case ConditionCode.NotSigned: return ConditionCode.Signed;
				case ConditionCode.Carry: return ConditionCode.NoCarry;
				case ConditionCode.NoCarry: return ConditionCode.Carry;
				case ConditionCode.Overflow: return ConditionCode.NoOverflow;
				case ConditionCode.NoOverflow: return ConditionCode.Overflow;
				case ConditionCode.Positive: return ConditionCode.Negative;
				case ConditionCode.Negative: return ConditionCode.Positive;
				case ConditionCode.Always: return ConditionCode.Never;
				case ConditionCode.Never: return ConditionCode.Always;
				case ConditionCode.Parity: return ConditionCode.NoParity;
				case ConditionCode.NoParity: return ConditionCode.Parity;
				default: throw new NotSupportedException();
			}
		}

		public static ConditionCode GetReverse(this ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return ConditionCode.Equal;
				case ConditionCode.NotEqual: return ConditionCode.NotEqual;
				case ConditionCode.GreaterOrEqual: return ConditionCode.LessOrEqual;
				case ConditionCode.GreaterThan: return ConditionCode.LessOrEqual;
				case ConditionCode.LessOrEqual: return ConditionCode.GreaterOrEqual;
				case ConditionCode.LessThan: return ConditionCode.GreaterThan;
				case ConditionCode.UnsignedGreaterOrEqual: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.UnsignedGreaterThan: return ConditionCode.UnsignedLessThan;
				case ConditionCode.UnsignedLessOrEqual: return ConditionCode.UnsignedGreaterOrEqual;
				case ConditionCode.UnsignedLessThan: return ConditionCode.UnsignedGreaterThan;
				case ConditionCode.Signed: return ConditionCode.Signed;
				case ConditionCode.NotSigned: return ConditionCode.NotSigned;
				case ConditionCode.Carry: return ConditionCode.Carry;
				case ConditionCode.NoCarry: return ConditionCode.NoCarry;
				case ConditionCode.Overflow: return ConditionCode.Overflow;
				case ConditionCode.NoOverflow: return ConditionCode.NoOverflow;
				case ConditionCode.Positive: return ConditionCode.Positive;
				case ConditionCode.Negative: return ConditionCode.Negative;
				case ConditionCode.Always: return ConditionCode.Always;
				case ConditionCode.Never: return ConditionCode.Never;
				case ConditionCode.Parity: return ConditionCode.Parity;
				case ConditionCode.NoParity: return ConditionCode.NoParity;
				default: throw new NotSupportedException();
			}
		}

		public static string GetConditionString(this ConditionCode conditioncode)
		{
			switch (conditioncode)
			{
				case ConditionCode.Equal: return "==";
				case ConditionCode.GreaterOrEqual: return ">=";
				case ConditionCode.GreaterThan: return ">";
				case ConditionCode.LessOrEqual: return "<=";
				case ConditionCode.LessThan: return "<";
				case ConditionCode.NotEqual: return "!=";
				case ConditionCode.UnsignedGreaterOrEqual: return ">= (U)";
				case ConditionCode.UnsignedGreaterThan: return "> (U)";
				case ConditionCode.UnsignedLessOrEqual: return "<= (U)";
				case ConditionCode.UnsignedLessThan: return "< (U)";
				case ConditionCode.NotSigned: return "not signed";
				case ConditionCode.Signed: return "signed";
				case ConditionCode.Zero: return "zero";
				case ConditionCode.NotZero: return "not zero";
				case ConditionCode.Carry: return "carry";
				case ConditionCode.NoCarry: return "no carry";
				case ConditionCode.Always: return "always";
				case ConditionCode.Parity: return "parity";
				case ConditionCode.NoParity: return "no parity";
				default: throw new NotSupportedException();
			}
		}

		#endregion Utility Methods
	}
}
