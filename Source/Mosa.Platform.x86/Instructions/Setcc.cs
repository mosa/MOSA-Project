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

		private static readonly OpCode E = new OpCode(new byte[] { 0x0F, 0x94 });
		private static readonly OpCode LT = new OpCode(new byte[] { 0x0F, 0x9C });
		private static readonly OpCode LE = new OpCode(new byte[] { 0x0F, 0x9E });
		private static readonly OpCode GE = new OpCode(new byte[] { 0x0F, 0x9D });
		private static readonly OpCode GT = new OpCode(new byte[] { 0x0F, 0x9F });
		private static readonly OpCode NE = new OpCode(new byte[] { 0x0F, 0x95 });
		private static readonly OpCode UGE = new OpCode(new byte[] { 0x0F, 0x93 }); // SETAE, SETNB, SETNC
		private static readonly OpCode UGT = new OpCode(new byte[] { 0x0F, 0x97 }); // SETNBE, SETA
		private static readonly OpCode ULE = new OpCode(new byte[] { 0x0F, 0x96 }); // SETBE, SETNA
		private static readonly OpCode ULT = new OpCode(new byte[] { 0x0F, 0x92 }); // SETNAE, SETB, SETC
		private static readonly OpCode P = new OpCode(new byte[] { 0x0F, 0x9A });
		private static readonly OpCode NP = new OpCode(new byte[] { 0x0F, 0x9B });
		private static readonly OpCode NC = new OpCode(new byte[] { 0x0F, 0x93 });
		private static readonly OpCode C = new OpCode(new byte[] { 0x0F, 0x92 });
		private static readonly OpCode Z = new OpCode(new byte[] { 0x0F, 0x94 });
		private static readonly OpCode NZ = new OpCode(new byte[] { 0x0F, 0x95 });

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
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			OpCode opcode;

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

			emitter.Emit(opcode, node.Result, null);
		}

		#endregion Methods
	}
}
