/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 setcc instruction.
	/// </summary>
	public sealed class SetccInstruction : BaseInstruction
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

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Gets the condition code string.
		/// </summary>
		/// <returns>The string shortcut of the condition code.</returns>
		public static string GetInstructionString(IR.ConditionCode code)
		{
			switch (code) {
				case IR.ConditionCode.Equal: return @"e";
				case IR.ConditionCode.GreaterOrEqual: return @"ge";
				case IR.ConditionCode.GreaterThan: return @"g";
				case IR.ConditionCode.LessOrEqual: return @"le";
				case IR.ConditionCode.LessThan: return @"l";
				case IR.ConditionCode.NotEqual: return @"ne";
				case IR.ConditionCode.UnsignedGreaterOrEqual: return @"ae";
				case IR.ConditionCode.UnsignedGreaterThan: return @"a";
				case IR.ConditionCode.UnsignedLessOrEqual: return @"be";
				case IR.ConditionCode.UnsignedLessThan: return @"b";
				default: throw new NotSupportedException();
			}
		}

		#endregion // Methods

		#region Methods
		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			OpCode opcode;

			switch (ctx.ConditionCode) {
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
				default: throw new NotSupportedException();
			}

			emitter.Emit(opcode, ctx.Result, null, null);
		}

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString(Context context)
		{
			return base.ToString(context).Remove(7, 2).Insert(7, GetInstructionString(context.ConditionCode));
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
