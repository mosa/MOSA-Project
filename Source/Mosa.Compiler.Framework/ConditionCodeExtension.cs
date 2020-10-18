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
				case ConditionCode.Greater: return ConditionCode.UnsignedGreater;
				case ConditionCode.LessOrEqual: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.Less: return ConditionCode.UnsignedLess;
				case ConditionCode.UnsignedGreaterOrEqual: break;
				case ConditionCode.UnsignedGreater: break;
				case ConditionCode.UnsignedLessOrEqual: break;
				case ConditionCode.UnsignedLess: break;
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
				case ConditionCode.GreaterOrEqual: return ConditionCode.Less;
				case ConditionCode.Greater: return ConditionCode.LessOrEqual;
				case ConditionCode.LessOrEqual: return ConditionCode.Greater;
				case ConditionCode.Less: return ConditionCode.GreaterOrEqual;
				case ConditionCode.UnsignedGreaterOrEqual: return ConditionCode.UnsignedLess;
				case ConditionCode.UnsignedGreater: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.UnsignedLessOrEqual: return ConditionCode.UnsignedGreater;
				case ConditionCode.UnsignedLess: return ConditionCode.UnsignedGreaterOrEqual;
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
				case ConditionCode.GreaterOrEqual: return ConditionCode.LessOrEqual;
				case ConditionCode.Greater: return ConditionCode.Less;
				case ConditionCode.LessOrEqual: return ConditionCode.GreaterOrEqual;
				case ConditionCode.Less: return ConditionCode.Greater;
				case ConditionCode.UnsignedGreaterOrEqual: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.UnsignedGreater: return ConditionCode.UnsignedLess;
				case ConditionCode.UnsignedLessOrEqual: return ConditionCode.UnsignedGreaterOrEqual;
				case ConditionCode.UnsignedLess: return ConditionCode.UnsignedGreater;
				default: return conditionCode;
			}
		}

		public static string GetConditionString(this ConditionCode conditioncode)
		{
			switch (conditioncode)
			{
				case ConditionCode.Equal: return "==";
				case ConditionCode.GreaterOrEqual: return ">=";
				case ConditionCode.Greater: return ">";
				case ConditionCode.LessOrEqual: return "<=";
				case ConditionCode.Less: return "<";
				case ConditionCode.NotEqual: return "!=";
				case ConditionCode.UnsignedGreaterOrEqual: return ">= (U)";
				case ConditionCode.UnsignedGreater: return "> (U)";
				case ConditionCode.UnsignedLessOrEqual: return "<= (U)";
				case ConditionCode.UnsignedLess: return "< (U)";
				case ConditionCode.NotSigned: return "not signed";
				case ConditionCode.Signed: return "signed";
				case ConditionCode.Zero: return "zero";
				case ConditionCode.NotZero: return "not zero";
				case ConditionCode.Carry: return "carry";
				case ConditionCode.NoCarry: return "no carry";
				case ConditionCode.Always: return "always";
				case ConditionCode.Parity: return "parity";
				case ConditionCode.NoParity: return "no parity";
				case ConditionCode.Positive: return "positive";
				case ConditionCode.Negative: return "negative";
				default: throw new NotSupportedException();
			}
		}

		#endregion Utility Methods
	}
}
