// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Specifies condition codes for <see cref="ConditionCode" />.
	/// </summary>
	public enum ConditionCode
	{
		/// <summary>
		/// The undefined
		/// </summary>
		Undefined,

		/// <summary>
		/// Equality comparison.
		/// </summary>
		Equal,

		/// <summary>
		/// Not equal comparison.
		/// </summary>
		NotEqual,

		/// <summary>
		/// Greater-than comparison.
		/// </summary>
		Greater,

		/// <summary>
		/// Greater-than or equal comparison.
		/// </summary>
		GreaterOrEqual,

		/// <summary>
		/// Less-than comparison.
		/// </summary>
		Less,

		/// <summary>
		/// Less-than or equal comparison.
		/// </summary>
		LessOrEqual,

		/// <summary>
		/// Unsigned greater than comparison.
		/// </summary>
		UnsignedGreater,

		/// <summary>
		/// Unsigned greater than or equal comparison.
		/// </summary>
		UnsignedGreaterOrEqual,

		/// <summary>
		/// Unsigned less than comparison.
		/// </summary>
		UnsignedLess,

		/// <summary>
		/// Unsigned less than or equal comparison.
		/// </summary>
		UnsignedLessOrEqual,

		/// <summary>
		/// Not unsigned
		/// </summary>
		NotSigned,

		/// <summary>
		/// signed
		/// </summary>
		Signed,

		/// <summary>
		/// Carry flag
		/// </summary>
		Carry,

		/// <summary>
		/// No carry flag
		/// </summary>
		NoCarry,

		/// <summary>
		/// Zero flag
		/// </summary>
		Zero,

		/// <summary>
		/// No zero flag
		/// </summary>
		NotZero,

		/// <summary>
		/// Overflow flag
		/// </summary>
		Overflow,

		/// <summary>
		/// No Overflow flag
		/// </summary>
		NoOverflow,

		/// <summary>
		/// Always
		/// </summary>
		Always,

		/// <summary>
		/// Never
		/// </summary>
		Never,

		/// <summary>
		/// The positive
		/// </summary>
		Positive,

		/// <summary>
		/// The negative
		/// </summary>
		Negative,

		/// <summary>
		/// No parity
		/// </summary>
		NoParity,

		/// <summary>
		/// Parity
		/// </summary>
		Parity,
	}
}
