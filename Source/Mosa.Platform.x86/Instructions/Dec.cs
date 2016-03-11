// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 int instruction.
	/// </summary>
	public sealed class Dec : X86Instruction
	{
		#region Data Members

		private static readonly OpCode DEC8 = new OpCode(new byte[] { 0xFE }, 1);
		private static readonly OpCode DEC16 = new OpCode(new byte[] { 0x66, 0xFF }, 1);
		private static readonly OpCode DEC32 = new OpCode(new byte[] { 0xFF }, 1);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Dec"/>.
		/// </summary>
		public Dec() :
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
			if (destination.IsByte) return DEC8;
			if (destination.IsShort || destination.IsChar) return DEC16;
			if (destination.IsInt) return DEC32;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(node.Result, null, null);
			emitter.Emit(opCode, node.Result);
		}

		#endregion Methods
	}
}
