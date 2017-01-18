// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 setcc instruction.
	/// </summary>
	public sealed class Setcc : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode E = new LegacyOpCode(new byte[] { 0x0F, 0x94 });
		private static readonly LegacyOpCode LT = new LegacyOpCode(new byte[] { 0x0F, 0x9C });
		private static readonly LegacyOpCode LE = new LegacyOpCode(new byte[] { 0x0F, 0x9E });
		private static readonly LegacyOpCode GE = new LegacyOpCode(new byte[] { 0x0F, 0x9D });
		private static readonly LegacyOpCode GT = new LegacyOpCode(new byte[] { 0x0F, 0x9F });
		private static readonly LegacyOpCode NE = new LegacyOpCode(new byte[] { 0x0F, 0x95 });
		private static readonly LegacyOpCode UGE = new LegacyOpCode(new byte[] { 0x0F, 0x93 }); // SETAE, SETNB, SETNC
		private static readonly LegacyOpCode UGT = new LegacyOpCode(new byte[] { 0x0F, 0x97 }); // SETNBE, SETA
		private static readonly LegacyOpCode ULE = new LegacyOpCode(new byte[] { 0x0F, 0x96 }); // SETBE, SETNA
		private static readonly LegacyOpCode ULT = new LegacyOpCode(new byte[] { 0x0F, 0x92 }); // SETNAE, SETB, SETC
		private static readonly LegacyOpCode P = new LegacyOpCode(new byte[] { 0x0F, 0x9A });
		private static readonly LegacyOpCode NP = new LegacyOpCode(new byte[] { 0x0F, 0x9B });
		private static readonly LegacyOpCode NC = new LegacyOpCode(new byte[] { 0x0F, 0x93 });
		private static readonly LegacyOpCode C = new LegacyOpCode(new byte[] { 0x0F, 0x92 });
		private static readonly LegacyOpCode Z = new LegacyOpCode(new byte[] { 0x0F, 0x94 });
		private static readonly LegacyOpCode NZ = new LegacyOpCode(new byte[] { 0x0F, 0x95 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Setcc"/> class.
		/// </summary>
		public Setcc()
			: base(1, 0)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			LegacyOpCode opcode;

			switch (node.ConditionCode)
			{
				case ConditionCode.Equal: opcode = E; break;
				case ConditionCode.LessThan: opcode = LT; break;
				case ConditionCode.LessOrEqual: opcode = LE; break;
				case ConditionCode.GreaterOrEqual: opcode = GE; break;
				case ConditionCode.GreaterThan: opcode = GT; break;
				case ConditionCode.NotEqual: opcode = NE; break;
				case ConditionCode.UnsignedGreaterOrEqual: opcode = UGE; break;
				case ConditionCode.UnsignedGreaterThan: opcode = UGT; break;
				case ConditionCode.UnsignedLessOrEqual: opcode = ULE; break;
				case ConditionCode.UnsignedLessThan: opcode = ULT; break;
				case ConditionCode.Parity: opcode = P; break;
				case ConditionCode.NoParity: opcode = NP; break;
				case ConditionCode.NoCarry: opcode = NC; break;
				case ConditionCode.Carry: opcode = C; break;
				case ConditionCode.Zero: opcode = Z; break;
				case ConditionCode.NotZero: opcode = NZ; break;
				default: throw new NotSupportedException();
			}

			emitter.Emit(opcode, node.Result);
		}

		#endregion Methods
	}
}
