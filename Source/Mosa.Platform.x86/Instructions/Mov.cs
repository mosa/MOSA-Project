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

		private static readonly LegacyOpCode RM_C = new LegacyOpCode(new byte[] { 0xC7 }, 0); // Move imm32 to r/m32
		private static readonly LegacyOpCode R_RM = new LegacyOpCode(new byte[] { 0x8B });
		private static readonly LegacyOpCode SEG_RM = new LegacyOpCode(new byte[] { 0x8E }); // Move r/m to seg
		private static readonly LegacyOpCode RM_SEG = new LegacyOpCode(new byte[] { 0x8C }); // Move seg to r/m

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
		internal override LegacyOpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (destination.Register is SegmentRegister)
			{
				if (source.IsCPURegister)
					return SEG_RM;

				throw new ArgumentException("TODO: No opcode for move destination segment register");
			}

			if (source.Register is SegmentRegister)
			{
				if (destination.IsCPURegister)
					return RM_SEG;

				throw new ArgumentException("TODO: No opcode for move source segment register");
			}

			if (destination.IsCPURegister && source.IsConstant)
				return RM_C;

			if (destination.IsCPURegister && source.IsCPURegister)
				return R_RM;

			throw new ArgumentException("No opcode for operand type. [" + destination + ", " + source + ")");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			var opCode = ComputeOpCode(node.Result, node.Operand1, null);
			emitter.Emit(opCode, node.Result, node.Operand1);
		}

		#endregion Methods
	}
}
