// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.ARM64;

/// <summary>
/// ARM64 Instruction
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformInstruction" />
public abstract class ARM64Instruction : BasePlatformInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="ARM32Instruction" /> class.
	/// </summary>
	/// <param name="resultCount">The result count.</param>
	/// <param name="operandCount">The operand count.</param>
	protected ARM64Instruction(byte resultCount, byte operandCount)
		: base(resultCount, operandCount)
	{
	}

	#endregion Construction

	/// <summary>
	/// Gets the name of the instruction family.
	/// </summary>
	/// <value>
	/// The name of the instruction family.
	/// </value>
	public override string FamilyName => "ARM64";

	public static byte GetConditionCode(ConditionCode condition)
	{
		return condition switch
		{
			ConditionCode.Always => 0b1110,
			ConditionCode.Zero => 0b0000,
			ConditionCode.NotZero => 0b0001,
			ConditionCode.Carry => 0b0010,
			ConditionCode.NoCarry => 0b0011,
			ConditionCode.Signed => 0b0100,
			ConditionCode.NotSigned => 0b0101,
			ConditionCode.Overflow => 0b0110,
			ConditionCode.NoOverflow => 0b0111,
			ConditionCode.Equal => 0b0000,
			ConditionCode.NotEqual => 0b0001,
			ConditionCode.GreaterOrEqual => 0b1010,
			ConditionCode.Greater => 0b1100,
			ConditionCode.LessOrEqual => 0b1101,
			ConditionCode.Less => 0b1011,
			ConditionCode.UnsignedGreaterOrEqual => 0b0010,
			ConditionCode.UnsignedGreater => 0b1000,
			ConditionCode.UnsignedLessOrEqual => 0b1001,
			ConditionCode.UnsignedLess => 0b0011,
			ConditionCode.Positive => 0b0101,
			ConditionCode.Never => 0b1111,
			ConditionCode.Undefined => 0b1110,
			_ => throw new NotSupportedException()
		};
	}
}
