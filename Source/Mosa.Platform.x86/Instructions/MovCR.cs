// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 movCR instruction.
	/// </summary>
	public sealed class MovCR : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode R_CR = new LegacyOpCode(new byte[] { 0x0F, 0x20 });
		private static readonly LegacyOpCode CR_R = new LegacyOpCode(new byte[] { 0x0F, 0x22 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovCR" />.
		/// </summary>
		public MovCR() :
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
			if (destination.Register is ControlRegister) return CR_R;
			if (source.Register is ControlRegister) return R_CR;

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

			if (node.Result.Register is ControlRegister)
			{
				emitter.Emit(opCode, node.Result, node.Operand1);
			}
			else
			{
				emitter.Emit(opCode, node.Operand1, node.Result);
			}
		}

		#endregion Methods
	}
}
