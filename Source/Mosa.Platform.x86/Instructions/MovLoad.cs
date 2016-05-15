// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 mov instruction.
	/// </summary>
	public sealed class MovLoad : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R_RM_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode R_M_U8 = new OpCode(new byte[] { 0x8A }); // Move r/m8 to R8
		private static readonly OpCode R_RM = new OpCode(new byte[] { 0x8B });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovLoad"/>.
		/// </summary>
		public MovLoad() :
			base(1, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">@No opcode for operand type. [ + destination + ,  + source + )</exception>
		private OpCode ComputeOpCode(InstructionSize size, Operand destination, Operand source)
		{
			Debug.Assert(destination.IsRegister);
			Debug.Assert(source.IsMemoryAddress);

			size = BaseMethodCompilerStage.GetInstructionSize(size, destination);

			Debug.Assert(size != InstructionSize.Size64);

			if (size == InstructionSize.Size8)
				return R_M_U8;

			if (size == InstructionSize.Size16)
				return R_RM_16;

			return R_RM;
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			var opCode = ComputeOpCode(node.Size, node.Result, node.Operand1);
			emitter.Emit(opCode, node.Result, node.Operand1);
		}

		#endregion Methods
	}
}
