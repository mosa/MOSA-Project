// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// X86Instruction
	/// </summary>
	public abstract class X86Instruction : BasePlatformInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected X86Instruction(byte resultCount, byte operandCount)
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
		public override string FamilyName { get { return "X86"; } }

		#endregion Properties

		public static byte GetConditionCode(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Equal: return 0x4;          // Equal (ZF = 1)
				case ConditionCode.NotEqual: return 0x5;       // NotEqual (ZF = 0)
				case ConditionCode.Zero: return 0x4;           // Zero (ZF = 1)
				case ConditionCode.NotZero: return 0x5;        // NotEqual (ZF = 0)
				case ConditionCode.GreaterOrEqual: return 0xD; // GreaterOrEqual (SF = OF)
				case ConditionCode.Greater: return 0xF;    // GreaterThan (ZF = 0 and SF = OF)
				case ConditionCode.LessOrEqual: return 0xE;    // LessOrEqual (ZF = 1 or SF <> OF)
				case ConditionCode.Less: return 0xC;       // LessThan (SF <> OF)
				case ConditionCode.UnsignedGreaterOrEqual: return 0x3; // UnsignedGreaterOrEqual (CF = 0)
				case ConditionCode.UnsignedGreater: return 0x7;    // UnsignedGreaterThan (CF = 0 & ZF = 0)
				case ConditionCode.UnsignedLessOrEqual: return 0x6;    // UnsignedLessOrEqual (CF = 1 or ZF = 1)
				case ConditionCode.UnsignedLess: return 0x2;       // UnsignedLessThan (CF = 1)
				case ConditionCode.Signed: return 0x8;         // Signed (SF = 1)
				case ConditionCode.NotSigned: return 0x9;      // NotSigned (SF = 0)
				case ConditionCode.Carry: return 0x2;          // Carry (CF = 1)
				case ConditionCode.NoCarry: return 0x3;        // NoCarry (CF = 0)
				case ConditionCode.Overflow: return 0x0;       // Overflow (OF = 1)
				case ConditionCode.NoOverflow: return 0x1;     // NoOverflow (OF = 0)
				case ConditionCode.Parity: return 0xA;         // Parity (PF = 1)
				case ConditionCode.NoParity: return 0xB;       // NoParity (PF = 0)
				default: throw new NotSupportedException();
			}
		}

		public static bool IsUsedZeroFlagWithCondition(ConditionCode condition)
		{
			return condition == ConditionCode.Equal
				|| condition == ConditionCode.NotEqual
				|| condition == ConditionCode.Zero
				|| condition == ConditionCode.NotZero
				|| condition == ConditionCode.UnsignedGreater
				|| condition == ConditionCode.UnsignedLessOrEqual;
		}

		public static bool IsCarryFlagUsedWithCondition(ConditionCode condition)
		{
			return condition == ConditionCode.UnsignedGreaterOrEqual
				|| condition == ConditionCode.UnsignedGreater
				|| condition == ConditionCode.UnsignedLessOrEqual
				|| condition == ConditionCode.UnsignedLess
				|| condition == ConditionCode.Carry
				|| condition == ConditionCode.NoCarry;
		}

		public static bool IsOverflowFlagUsedWithCondition(ConditionCode condition)
		{
			return condition == ConditionCode.Overflow
				|| condition == ConditionCode.NoOverflow
				|| condition == ConditionCode.GreaterOrEqual
				|| condition == ConditionCode.Greater
				|| condition == ConditionCode.LessOrEqual
				|| condition == ConditionCode.Less;
		}

		public static bool IsSignFlagUsedWithCondition(ConditionCode condition)
		{
			return condition == ConditionCode.GreaterOrEqual
				|| condition == ConditionCode.Greater
				|| condition == ConditionCode.LessOrEqual
				|| condition == ConditionCode.Less
				|| condition == ConditionCode.Signed
				|| condition == ConditionCode.NotSigned;
		}

		public static bool IsParityFlagUsedWithCondition(ConditionCode condition)
		{
			return condition == ConditionCode.Parity
				|| condition == ConditionCode.NoParity;
		}
	}
}
