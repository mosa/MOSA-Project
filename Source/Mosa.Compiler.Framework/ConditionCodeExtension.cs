// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework;

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
		return conditionCode switch
		{
			ConditionCode.Equal => ConditionCode.NotEqual,
			ConditionCode.NotEqual => ConditionCode.Equal,
			ConditionCode.GreaterOrEqual => ConditionCode.Less,
			ConditionCode.Greater => ConditionCode.LessOrEqual,
			ConditionCode.LessOrEqual => ConditionCode.Greater,
			ConditionCode.Less => ConditionCode.GreaterOrEqual,
			ConditionCode.UnsignedGreaterOrEqual => ConditionCode.UnsignedLess,
			ConditionCode.UnsignedGreater => ConditionCode.UnsignedLessOrEqual,
			ConditionCode.UnsignedLessOrEqual => ConditionCode.UnsignedGreater,
			ConditionCode.UnsignedLess => ConditionCode.UnsignedGreaterOrEqual,
			ConditionCode.Signed => ConditionCode.NotSigned,
			ConditionCode.NotSigned => ConditionCode.Signed,
			ConditionCode.Carry => ConditionCode.NoCarry,
			ConditionCode.NoCarry => ConditionCode.Carry,
			ConditionCode.Overflow => ConditionCode.NoOverflow,
			ConditionCode.NoOverflow => ConditionCode.Overflow,
			ConditionCode.Positive => ConditionCode.Negative,
			ConditionCode.Negative => ConditionCode.Positive,
			ConditionCode.Always => ConditionCode.Never,
			ConditionCode.Never => ConditionCode.Always,
			ConditionCode.Parity => ConditionCode.NoParity,
			ConditionCode.NoParity => ConditionCode.Parity,
			_ => throw new NotSupportedException()
		};
	}

	public static ConditionCode GetReverse(this ConditionCode conditionCode)
	{
		return conditionCode switch
		{
			ConditionCode.GreaterOrEqual => ConditionCode.LessOrEqual,
			ConditionCode.Greater => ConditionCode.Less,
			ConditionCode.LessOrEqual => ConditionCode.GreaterOrEqual,
			ConditionCode.Less => ConditionCode.Greater,
			ConditionCode.UnsignedGreaterOrEqual => ConditionCode.UnsignedLessOrEqual,
			ConditionCode.UnsignedGreater => ConditionCode.UnsignedLess,
			ConditionCode.UnsignedLessOrEqual => ConditionCode.UnsignedGreaterOrEqual,
			ConditionCode.UnsignedLess => ConditionCode.UnsignedGreater,
			_ => conditionCode
		};
	}

	public static string GetConditionString(this ConditionCode conditioncode)
	{
		return conditioncode switch
		{
			ConditionCode.Equal => "==",
			ConditionCode.GreaterOrEqual => ">=",
			ConditionCode.Greater => ">",
			ConditionCode.LessOrEqual => "<=",
			ConditionCode.Less => "<",
			ConditionCode.NotEqual => "!=",
			ConditionCode.UnsignedGreaterOrEqual => ">= (U)",
			ConditionCode.UnsignedGreater => "> (U)",
			ConditionCode.UnsignedLessOrEqual => "<= (U)",
			ConditionCode.UnsignedLess => "< (U)",
			ConditionCode.NotSigned => "not signed",
			ConditionCode.Signed => "signed",
			ConditionCode.Zero => "zero",
			ConditionCode.NotZero => "not zero",
			ConditionCode.Carry => "carry",
			ConditionCode.NoCarry => "no carry",
			ConditionCode.Overflow => "overflow",
			ConditionCode.NoOverflow => "no overflow",
			ConditionCode.Always => "always",
			ConditionCode.Parity => "parity",
			ConditionCode.NoParity => "no parity",
			ConditionCode.Positive => "positive",
			ConditionCode.Negative => "negative",
			_ => throw new NotSupportedException()
		};
	}

	#endregion Utility Methods
}
