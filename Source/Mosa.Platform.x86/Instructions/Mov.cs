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
	/// Representations the x86 mov instruction.
	/// </summary>
	public sealed class Mov : X86Instruction
	{
		#region Data Members

		private static readonly OpCode RM_C = new OpCode(new byte[] { 0xC7 }, 0); // Move imm32 to r/m32
		private static readonly OpCode RM_C_U8 = new OpCode(new byte[] { 0xC6 }, 0); // Move imm8 to r/m8
		private static readonly OpCode R_R_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode RM_R_U8 = new OpCode(new byte[] { 0x88 });
		private static readonly OpCode R_RM = new OpCode(new byte[] { 0x8B });
		private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x89 });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x89 });
		private static readonly OpCode R_M_U8 = new OpCode(new byte[] { 0x8A }); // Move r/m8 to R8
		private static readonly OpCode SR_R = new OpCode(new byte[] { 0x8E });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mov"/>.
		/// </summary>
		public Mov() :
			base(1, 1)
		{
		}

		#endregion Construction

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
			if (destination.IsRegister && destination.Register is SegmentRegister) return SR_R;

			if (source.IsRegister && source.Register is SegmentRegister)
				throw new ArgumentException(@"TODO: No opcode for move from segment register");

			if (destination.IsRegister && source.IsConstant) return RM_C;

			if (destination.IsMemoryAddress && source.IsConstant)
			{
				if (source.IsByte) return RM_C_U8;
				return RM_C;
			}

			if (destination.IsRegister && source.IsSymbol) return RM_C;

			if (destination.IsMemoryAddress && source.IsSymbol) return RM_C;

			if (destination.IsRegister && source.IsRegister)
			{
				if (source.IsByte || destination.IsByte) return R_M_U8;
				if (source.IsChar || destination.IsChar || source.IsShort || destination.IsShort) return R_R_16;
				return R_RM;
			}

			if (destination.IsRegister && source.IsMemoryAddress)
			{
				if (destination.IsByte) return R_M_U8;
				if (destination.IsChar || destination.IsShort) return R_M_16;
				return R_RM;
			}

			if (destination.IsMemoryAddress && source.IsRegister)
			{
				if (destination.IsByte) return RM_R_U8;
				if (destination.IsChar || destination.IsShort) return M_R_16;
				return M_R;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination + ", " + source + ")");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, context.Operand1, null);
			emitter.Emit(opCode, context.Result, context.Operand1);
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

		#endregion Methods
	}
}