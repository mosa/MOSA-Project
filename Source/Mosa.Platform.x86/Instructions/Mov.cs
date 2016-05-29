// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

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
		private static readonly OpCode R_RM_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode RM_R_U8 = new OpCode(new byte[] { 0x88 });
		private static readonly OpCode R_RM = new OpCode(new byte[] { 0x8B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x89 });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x89 });
		private static readonly OpCode RM_U8 = new OpCode(new byte[] { 0x8A }); // Move r/m8 to R8
		private static readonly OpCode M_C_16 = new OpCode(new byte[] { 0x66, 0xC7 });
		private static readonly OpCode SEG_RM = new OpCode(new byte[] { 0x8E }); // Move r/m to seg
		private static readonly OpCode RM_SEG = new OpCode(new byte[] { 0x8C }); // Move seg to r/m

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
			if (destination.Register is SegmentRegister)
			{
				if (source.IsRegister || destination.IsMemoryAddress) return SEG_RM;

				throw new ArgumentException(@"TODO: No opcode for move destination segment register");
			}

			if (source.Register is SegmentRegister)
			{
				if (destination.IsRegister || destination.IsMemoryAddress) return RM_SEG;

				throw new ArgumentException(@"TODO: No opcode for move source segment register");
			}

			if (destination.IsRegister && source.IsConstant) return RM_C;

			if (destination.IsMemoryAddress && source.IsConstant)
			{
				if (destination.IsByte || destination.IsBoolean) return RM_C_U8;
				if (destination.IsChar || destination.IsShort) return M_C_16;
				return RM_C;
			}

			if (destination.IsRegister && source.IsSymbol) return RM_C;

			if (destination.IsMemoryAddress && source.IsSymbol) return RM_C;

			if (destination.IsRegister && source.IsRegister)
			{
				Debug.Assert(!((source.IsByte || destination.IsByte) && (source.Register == GeneralPurposeRegister.ESI || source.Register == GeneralPurposeRegister.EDI)), source.ToString());

				if (source.IsByte || destination.IsByte) return RM_U8;
				if (source.IsChar || destination.IsChar || source.IsShort || destination.IsShort) return R_RM_16;
				return R_RM;
			}

			if (destination.IsRegister && source.IsMemoryAddress)
			{
				if (destination.IsByte || destination.IsBoolean) return RM_U8;
				if (destination.IsChar || destination.IsShort) return R_RM_16;
				return R_RM;
			}

			if (destination.IsMemoryAddress && source.IsRegister)
			{
				if (destination.IsByte || destination.IsBoolean) return RM_R_U8;
				if (destination.IsChar || destination.IsShort) return M_R_16;
				return M_R;
			}

			if (destination.IsSymbol && source.IsRegister)
			{
				if (destination.IsByte || destination.IsBoolean) return RM_C_U8;
				if (destination.IsChar || destination.IsShort) return M_C_16;
				return RM_C;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination + ", " + source + ")");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(node.Result, node.Operand1, null);
			emitter.Emit(opCode, node.Result, node.Operand1);
		}

		#endregion Methods
	}
}
