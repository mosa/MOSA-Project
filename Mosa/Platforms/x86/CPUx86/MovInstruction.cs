/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 mov instruction.
	/// </summary>
	public sealed class MovInstruction : TwoOperandInstruction, IIntrinsicInstruction
	{
		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0xC7 }, 0); // Move imm32 to r/m32
		private static readonly OpCode M_C = new OpCode(new byte[] { 0xC7 }, 0);
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x8B });
		private static readonly OpCode R_R_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode R_R_U8 = new OpCode(new byte[] { 0x88 });
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x8B }); // Move r/m32 to R32
		private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x89 });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x89 });
		private static readonly OpCode M_R_U8 = new OpCode(new byte[] { 0x88 }); // Move R8 to r/rm8
		private static readonly OpCode R_M_U8 = new OpCode(new byte[] { 0x8A }); // Move r/m8 to R8

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C;
			if ((destination is MemoryOperand) && (source is ConstantOperand)) return M_C;
			if ((destination is RegisterOperand) && (source is RegisterOperand)) {
				if (IsByte(source) || IsByte(destination)) return R_R_U8;
				if (IsChar(source) || IsChar(destination) || IsShort(source) || IsShort(destination)) return R_R_16;
				return R_R;
			}

			if ((destination is RegisterOperand) && (source is MemoryOperand)) {
				if (IsByte(destination)) return R_M_U8;
				if (IsChar(destination) || IsShort(destination)) return R_M_16;
				return R_M;
			}

			if ((destination is MemoryOperand) && (source is RegisterOperand)) {
				if (IsByte(destination)) return M_R_U8;
				if (IsChar(destination) || IsShort(destination)) return M_R_16;
				return M_R;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Mov(context);
		}

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			context.SetInstruction(CPUx86.Instruction.MovInstruction, context.Result, context.Operand1);
		}

		#endregion
	}
}
