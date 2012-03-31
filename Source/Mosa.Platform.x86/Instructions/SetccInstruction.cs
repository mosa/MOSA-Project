/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Compiler.Framework;
using IR = Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 setcc instruction.
	/// </summary>
	public sealed class SetccInstruction : X86Instruction
	{

		#region Data Members

		private static readonly OpCode E = new OpCode(new byte[] { 0x0F, 0x94 });
		private static readonly OpCode LT = new OpCode(new byte[] { 0x0F, 0x9C });
		private static readonly OpCode LE = new OpCode(new byte[] { 0x0F, 0x9E });
		private static readonly OpCode GE = new OpCode(new byte[] { 0x0F, 0x9D });
		private static readonly OpCode GT = new OpCode(new byte[] { 0x0F, 0x9F });
		private static readonly OpCode NE = new OpCode(new byte[] { 0x0F, 0x95 });
		private static readonly OpCode UGE = new OpCode(new byte[] { 0x0F, 0x93 });
		private static readonly OpCode UGT = new OpCode(new byte[] { 0x0F, 0x97 });
		private static readonly OpCode ULE = new OpCode(new byte[] { 0x0F, 0x96 });
		private static readonly OpCode ULT = new OpCode(new byte[] { 0x0F, 0x92 });
		private static readonly OpCode P = new OpCode(new byte[] { 0x0F, 0x9A });
		private static readonly OpCode NP = new OpCode(new byte[] { 0x0F, 0x9B });
		private static readonly OpCode NC = new OpCode(new byte[] { 0x0F, 0x93 });
		private static readonly OpCode C = new OpCode(new byte[] { 0x0F, 0x92 });
		private static readonly OpCode Z = new OpCode(new byte[] { 0x0F, 0x94 });
		private static readonly OpCode NZ = new OpCode(new byte[] { 0x0F, 0x95 });

		#endregion

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opcode;

			switch (context.ConditionCode)
			{
				case IR.ConditionCode.Equal: opcode = E; break;
				case IR.ConditionCode.LessThan: opcode = LT; break;
				case IR.ConditionCode.LessOrEqual: opcode = LE; break;
				case IR.ConditionCode.GreaterOrEqual: opcode = GE; break;
				case IR.ConditionCode.GreaterThan: opcode = GT; break;
				case IR.ConditionCode.NotEqual: opcode = NE; break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: opcode = UGE; break;
				case IR.ConditionCode.UnsignedGreaterThan: opcode = UGT; break;
				case IR.ConditionCode.UnsignedLessOrEqual: opcode = ULE; break;
				case IR.ConditionCode.UnsignedLessThan: opcode = ULT; break;
				case IR.ConditionCode.Parity: opcode = P; break;
				case IR.ConditionCode.NoParity: opcode = NP; break;
				case IR.ConditionCode.NoCarry: opcode = NC; break;
				case IR.ConditionCode.Carry: opcode = C; break;
				case IR.ConditionCode.Zero: opcode = Z; break;
				case IR.ConditionCode.NoZero: opcode = NZ; break;
				default: throw new NotSupportedException();
			}

			emitter.Emit(opcode, context.Result, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Setcc(context);
		}

		#endregion // Methods
	}
}
