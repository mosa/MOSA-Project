// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
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
				case ConditionCode.Parity: return ConditionCode.NoParity;
				case ConditionCode.NoParity: return ConditionCode.Parity;
				case ConditionCode.Carry: return ConditionCode.NoCarry;
				case ConditionCode.NoCarry: return ConditionCode.Carry;
				case ConditionCode.Overflow: return ConditionCode.NoOverflow;
				case ConditionCode.NoOverflow: return ConditionCode.Overflow;
				case ConditionCode.Positive: return ConditionCode.Negative;
				case ConditionCode.Negative: return ConditionCode.Positive;
				case ConditionCode.Always: return ConditionCode.Never;
				case ConditionCode.Never: return ConditionCode.Always;
				default: throw new NotSupportedException();
			}
		}

		#endregion Utility Methods
	}
}
