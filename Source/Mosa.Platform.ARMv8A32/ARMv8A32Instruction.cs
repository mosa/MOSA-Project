// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.ARMv8A32
{
	/// <summary>
	/// ARMv8A32 Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformInstruction" />
	public abstract class ARMv8A32Instruction : BasePlatformInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ARMv8A32Instruction" /> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected ARMv8A32Instruction(byte resultCount, byte operandCount)
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
		public override string FamilyName => "ARMv8A32";

		public static byte GetConditionCode(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Always: return 0b1110;

				case ConditionCode.Zero: return 0b0000;
				case ConditionCode.NotZero: return 0b0001;

				case ConditionCode.Carry: return 0b0010;
				case ConditionCode.NoCarry: return 0b0011;

				case ConditionCode.Signed: return 0b0100;
				case ConditionCode.NotSigned: return 0b0101;

				case ConditionCode.Overflow: return 0b0110;
				case ConditionCode.NoOverflow: return 0b0111;

				case ConditionCode.Equal: return 0b0000;
				case ConditionCode.NotEqual: return 0b0001;

				case ConditionCode.GreaterOrEqual: return 0b1010;
				case ConditionCode.Greater: return 0b1100;
				case ConditionCode.LessOrEqual: return 0b1101;
				case ConditionCode.Less: return 0b1011;
				case ConditionCode.UnsignedGreaterOrEqual: return 0b0010;
				case ConditionCode.UnsignedGreater: return 0b1000;
				case ConditionCode.UnsignedLessOrEqual: return 0b1001;
				case ConditionCode.UnsignedLess: return 0b0011;
				case ConditionCode.Positive: return 0b0101;

				case ConditionCode.Never: return 0b1111;
				case ConditionCode.Undefined: return 0b1110;
				default: throw new NotSupportedException();
			}
		}
	}
}
