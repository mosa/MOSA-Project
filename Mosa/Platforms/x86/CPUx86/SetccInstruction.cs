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
	public sealed class SetccInstruction : OneOperandInstruction
	{

		#region Data Members

		private static readonly byte[] E = new byte[] { 0x0F, 0x94 };
		private static readonly byte[] LT = new byte[] { 0x0F, 0x9C };
		private static readonly byte[] LE = new byte[] { 0x0F, 0x9E };
		private static readonly byte[] GE = new byte[] { 0x0F, 0x9D };
		private static readonly byte[] GT = new byte[] { 0x0F, 0x9F };
		private static readonly byte[] NE = new byte[] { 0x0F, 0x95 };
		private static readonly byte[] UGE = new byte[] { 0x0F, 0x93 };
		private static readonly byte[] UGT = new byte[] { 0x0F, 0x97 };
		private static readonly byte[] ULE = new byte[] { 0x0F, 0x96 };
		private static readonly byte[] ULT = new byte[] { 0x0F, 0x92 };

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
			byte[] byteCode;

			switch (ctx.ConditionCode) {
				case IR.ConditionCode.Equal: byteCode = E; break;
				case IR.ConditionCode.LessThan: byteCode = LT; break;
				case IR.ConditionCode.LessOrEqual: byteCode = LE; break;
				case IR.ConditionCode.GreaterOrEqual: byteCode = GE; break;
				case IR.ConditionCode.GreaterThan: byteCode = GT; break;
				case IR.ConditionCode.NotEqual: byteCode = NE; break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: byteCode = UGE; break;
				case IR.ConditionCode.UnsignedGreaterThan: byteCode = UGT; break;
				case IR.ConditionCode.UnsignedLessOrEqual: byteCode = ULE; break;
				case IR.ConditionCode.UnsignedLessThan: byteCode = ULT; break;
				default: throw new NotSupportedException();
			}

			emitter.Emit(new OpCode(byteCode), ctx.Operand1, null);
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
