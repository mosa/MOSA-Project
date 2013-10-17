/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representation a x86 Cmov instruction.
	/// </summary>
	public sealed class Cmov : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Cmov"/>.
		/// </summary>
		public Cmov() :
			base(0, 2)
		{
		}

		#endregion Construction

		#region Data Members

		private static readonly OpCode CMOVO = new OpCode(new byte[] { 0x0F, 0x40 });	// Overflow (OF = 1)
		private static readonly OpCode CMOVNO = new OpCode(new byte[] { 0x0F, 0x41 });	// NoOverflow (OF = 0)
		private static readonly OpCode CMOVB = new OpCode(new byte[] { 0x0F, 0x42 });	// UnsignedLessThan (CF = 1).
		private static readonly OpCode CMOVC = new OpCode(new byte[] { 0x0F, 0x42 });	// Carry (CF = 1)
		private static readonly OpCode CMOVNB = new OpCode(new byte[] { 0x0F, 0x43 });	// UnsignedGreaterOrEqual (greater or equal) (CF = 0)
		private static readonly OpCode CMOVNC = new OpCode(new byte[] { 0x0F, 0x43 });	// NoCarry (CF = 0)
		private static readonly OpCode CMOVE = new OpCode(new byte[] { 0x0F, 0x44 });	// Equal (ZF = 1)
		private static readonly OpCode CMOVZ = new OpCode(new byte[] { 0x0F, 0x44 });	// Zero (ZF = 1)
		private static readonly OpCode CMOVNE = new OpCode(new byte[] { 0x0F, 0x45 });	// NotEqual (ZF = 0)
		private static readonly OpCode CMOVNZ = new OpCode(new byte[] { 0x0F, 0x45 });	// NotZero (ZF = 1)
		private static readonly OpCode CMOVBE = new OpCode(new byte[] { 0x0F, 0x46 });	// UnsignedLessOrEqual (CF = 1 or ZF = 1).
		private static readonly OpCode CMOVA = new OpCode(new byte[] { 0x0F, 0x47 });	// UnsignedGreaterThan (CF = 0 and ZF = 0).
		private static readonly OpCode CMOVS = new OpCode(new byte[] { 0x0F, 0x48 });	// Signed (CF = 0 and ZF = 0)
		private static readonly OpCode CMOVNS = new OpCode(new byte[] { 0x0F, 0x49 });	// NotSigned (SF = 0)
		private static readonly OpCode CMOVP = new OpCode(new byte[] { 0x0F, 0x4A });	// Parity (PF = 1)
		private static readonly OpCode CMOVNP = new OpCode(new byte[] { 0x0F, 0x4B });	// NoParity (PF = 0)
		private static readonly OpCode CMOVL = new OpCode(new byte[] { 0x0F, 0x4C });	// LessThan (SF <> OF)
		private static readonly OpCode CMOVGE = new OpCode(new byte[] { 0x0F, 0x4D });	// GreaterOrEqual (greater or equal) (SF = OF)
		private static readonly OpCode CMOVLE = new OpCode(new byte[] { 0x0F, 0x4E });	// LessOrEqual (ZF = 1 or SF <> OF)
		private static readonly OpCode CMOVG = new OpCode(new byte[] { 0x0F, 0x4F });	// GreaterThan (ZF = 0 and SF = OF)

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opcode = null;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: opcode = CMOVO; break;
				case ConditionCode.NotEqual: opcode = CMOVNE; break;
				case ConditionCode.Zero: opcode = CMOVZ; break;
				case ConditionCode.NotZero: opcode = CMOVNZ; break;
				case ConditionCode.GreaterOrEqual: opcode = CMOVGE; break;
				case ConditionCode.GreaterThan: opcode = CMOVG; break;
				case ConditionCode.LessOrEqual: opcode = CMOVLE; break;
				case ConditionCode.LessThan: opcode = CMOVL; break;
				case ConditionCode.UnsignedGreaterOrEqual: opcode = CMOVNB; break;
				case ConditionCode.UnsignedGreaterThan: opcode = CMOVA; break;
				case ConditionCode.UnsignedLessOrEqual: opcode = CMOVBE; break;
				case ConditionCode.UnsignedLessThan: opcode = CMOVB; break;
				case ConditionCode.Signed: opcode = CMOVS; break;
				case ConditionCode.NotSigned: opcode = CMOVNS; break;
				case ConditionCode.Carry: opcode = CMOVC; break;
				case ConditionCode.NoCarry: opcode = CMOVNC; break;
				case ConditionCode.Overflow: opcode = CMOVO; break;
				case ConditionCode.NoOverflow: opcode = CMOVNO; break;
				case ConditionCode.Parity: opcode = CMOVP; break;
				case ConditionCode.NoParity: opcode = CMOVNP; break;
				default:
					throw new NotSupportedException();
			}

			emitter.Emit(opcode, context.Result, context.Operand1);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cmov(context);
		}

		#endregion Methods
	}
}