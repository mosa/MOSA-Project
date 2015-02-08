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
	/// Representation a x86 branch instruction.
	/// </summary>
	public sealed class Branch : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Branch"/>.
		/// </summary>
		public Branch() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Properties

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

		#endregion Properties

		#region Data Members

		private static readonly byte[] JO = new byte[] { 0x0F, 0x80 };	// Overflow (OF = 1)
		private static readonly byte[] JNO = new byte[] { 0x0F, 0x81 };	// NoOverflow (OF = 0)
		private static readonly byte[] JC = new byte[] { 0x0F, 0x82 };	// Carry (CF = 1)
		private static readonly byte[] JB = new byte[] { 0x0F, 0x82 };	// UnsignedLessThan (CF = 1)
		private static readonly byte[] JAE = new byte[] { 0x0F, 0x83 };	// UnsignedGreaterOrEqual (CF = 0)
		private static readonly byte[] JNC = new byte[] { 0x0F, 0x83 };	// NoCarry (CF = 0)
		private static readonly byte[] JE = new byte[] { 0x0F, 0x84 };	// Equal (ZF = 1)
		private static readonly byte[] JZ = new byte[] { 0x0F, 0x84 };	// Zero (ZF = 1)
		private static readonly byte[] JNE = new byte[] { 0x0F, 0x85 }; // NotEqual (ZF = 0)
		private static readonly byte[] JNZ = new byte[] { 0x0F, 0x85 }; // NotZero (ZF = 0)
		private static readonly byte[] JBE = new byte[] { 0x0F, 0x86 };	// UnsignedLessOrEqual (CF = 1 or ZF = 1)
		private static readonly byte[] JA = new byte[] { 0x0F, 0x87 };	// UnsignedGreaterThan (CF = 0 and ZF = 0)
		private static readonly byte[] JS = new byte[] { 0x0F, 0x88 };	// Signed (SF = 1)
		private static readonly byte[] JNS = new byte[] { 0x0F, 0x89 };	// NotSigned (SF = 0)
		private static readonly byte[] JP = new byte[] { 0x0F, 0x8A };	// Parity (PF = 1)
		private static readonly byte[] JNP = new byte[] { 0x0F, 0x8B };	// NoParity (PF = 0)
		private static readonly byte[] JL = new byte[] { 0x0F, 0x8C };	// LessThan (SF <> OF)
		private static readonly byte[] JGE = new byte[] { 0x0F, 0x8D };	// GreaterOrEqual (SF = OF)
		private static readonly byte[] JLE = new byte[] { 0x0F, 0x8E }; // LessOrEqual (ZF = 1 or SF <> OF)
		private static readonly byte[] JG = new byte[] { 0x0F, 0x8F };	// GreaterThan (ZF = 0 and SF = OF)

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			byte[] opcode = null;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: opcode = JE; break;
				case ConditionCode.NotEqual: opcode = JNE; break;
				case ConditionCode.Zero: opcode = JZ; break;
				case ConditionCode.NotZero: opcode = JNZ; break;
				case ConditionCode.GreaterOrEqual: opcode = JGE; break;
				case ConditionCode.GreaterThan: opcode = JG; break;
				case ConditionCode.LessOrEqual: opcode = JLE; break;
				case ConditionCode.LessThan: opcode = JL; break;
				case ConditionCode.UnsignedGreaterOrEqual: opcode = JAE; break;
				case ConditionCode.UnsignedGreaterThan: opcode = JA; break;
				case ConditionCode.UnsignedLessOrEqual: opcode = JBE; break;
				case ConditionCode.UnsignedLessThan: opcode = JB; break;
				case ConditionCode.Signed: opcode = JS; break;
				case ConditionCode.NotSigned: opcode = JNS; break;
				case ConditionCode.Carry: opcode = JC; break;
				case ConditionCode.NoCarry: opcode = JNC; break;
				case ConditionCode.Overflow: opcode = JO; break;
				case ConditionCode.NoOverflow: opcode = JNO; break;
				case ConditionCode.Parity: opcode = JP; break;
				case ConditionCode.NoParity: opcode = JNP; break;
				default: throw new NotSupportedException();
			}

			emitter.EmitRelativeBranch(opcode, context.Targets[0].Label);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Branch(context);
		}

		#endregion Methods
	}
}