// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 out instruction.
	/// </summary>
	public sealed class Out : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R_8 = new OpCode(new byte[] { 0xEE });
		private static readonly OpCode R_32 = new OpCode(new byte[] { 0xEF });

		private static readonly OpCode C_8 = new OpCode(new byte[] { 0xE6 });
		private static readonly OpCode C_32 = new OpCode(new byte[] { 0xE7 });

		#endregion Data Members

		/// <summary>
		/// Initializes a new instance of <see cref="Out"/>.
		/// </summary>
		public Out() :
			base(0, 2)
		{
		}

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException"></exception>
		private OpCode ComputeOpCode(InstructionSize size, Operand destination, Operand source)
		{
			Debug.Assert(destination.IsConstant || destination.IsCPURegister);
			Debug.Assert(size != InstructionSize.None);
			Debug.Assert(size != InstructionSize.Native);

			//size = BaseMethodCompilerStage.GetInstructionSize(size, destination);

			if (destination.IsCPURegister)
			{
				if (size == InstructionSize.Size8)
					return R_8;

				return R_32;
			}

			if (destination.IsConstant)
			{
				if (size == InstructionSize.Size8)
					return C_8;

				return C_32;
			}

			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			var opCode = ComputeOpCode(node.Size, node.Operand1, node.Operand2);

			if (node.Operand1.IsConstant)
			{
				emitter.Emit(opCode, node.Operand1, null);
			}
			else
			{
				emitter.Emit(opCode, null, null);
			}
		}

		#endregion Methods
	}
}
