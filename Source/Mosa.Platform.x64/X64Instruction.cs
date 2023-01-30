// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x64;

/// <summary>
/// X86Instruction
/// </summary>
public abstract class X64Instruction : BasePlatformInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="X64Instruction"/> class.
	/// </summary>
	/// <param name="resultCount">The result count.</param>
	/// <param name="operandCount">The operand count.</param>
	protected X64Instruction(byte resultCount, byte operandCount)
		: base(resultCount, operandCount)
	{
	}

	#endregion Construction

	#region Properties

	/// <summary>
	/// Gets the name of the instruction family.
	/// </summary>
	/// <value>
	/// The name of the instruction family.
	/// </value>
	public override string FamilyName => "X64";

	#endregion Properties

	public static byte GetConditionCode(ConditionCode condition)
	{
		return condition switch
		{
			ConditionCode.Equal => 0x4 // Equal (ZF = 1)
			,
			ConditionCode.NotEqual => 0x5 // NotEqual (ZF = 0)
			,
			ConditionCode.Zero => 0x4 // Zero (ZF = 1)
			,
			ConditionCode.NotZero => 0x5 // NotEqual (ZF = 0)
			,
			ConditionCode.GreaterOrEqual => 0xD // GreaterOrEqual (SF = OF)
			,
			ConditionCode.Greater => 0xF // GreaterThan (ZF = 0 and SF = OF)
			,
			ConditionCode.LessOrEqual => 0xE // LessOrEqual (ZF = 1 or SF <> OF)
			,
			ConditionCode.Less => 0xC // LessThan (SF <> OF)
			,
			ConditionCode.UnsignedGreaterOrEqual => 0x3 // UnsignedGreaterOrEqual (CF = 0)
			,
			ConditionCode.UnsignedGreater => 0x7 // UnsignedGreaterThan (CF = 0 and ZF = 0)
			,
			ConditionCode.UnsignedLessOrEqual => 0x6 // UnsignedLessOrEqual (CF = 1 or ZF = 1)
			,
			ConditionCode.UnsignedLess => 0x2 // UnsignedLessThan (CF = 1)
			,
			ConditionCode.Signed => 0x8 // Signed (SF = 1)
			,
			ConditionCode.NotSigned => 0x9 // NotSigned (SF = 0)
			,
			ConditionCode.Carry => 0x2 // Carry (CF = 1)
			,
			ConditionCode.NoCarry => 0x3 // NoCarry (CF = 0)
			,
			ConditionCode.Overflow => 0x0 // Overflow (OF = 1)
			,
			ConditionCode.NoOverflow => 0x1 // NoOverflow (OF = 0)
			,
			ConditionCode.Parity => 0xA // Parity (PF = 1)
			,
			ConditionCode.NoParity => 0xB // NoParity (PF = 0)
			,
			_ => throw new NotSupportedException()
		};
	}
}
