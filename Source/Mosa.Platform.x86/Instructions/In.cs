// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 in instruction.
	/// </summary>
	public sealed class In : TwoOperandInstruction
	{
		#region Data Members

		private static readonly LegacyOpCode C_8 = new LegacyOpCode(new byte[] { 0xE4 });
		private static readonly LegacyOpCode R_8 = new LegacyOpCode(new byte[] { 0xEC });
		private static readonly LegacyOpCode C_16 = new LegacyOpCode(new byte[] { 0x66, 0xE5 });
		private static readonly LegacyOpCode R_16 = new LegacyOpCode(new byte[] { 0x66, 0xED });
		private static readonly LegacyOpCode C_32 = new LegacyOpCode(new byte[] { 0xE5 });
		private static readonly LegacyOpCode R_32 = new LegacyOpCode(new byte[] { 0xED });

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException"></exception>
		private LegacyOpCode ComputeOpCode(InstructionSize size, Operand destination, Operand source)
		{
			Debug.Assert(destination.IsConstant || destination.IsCPURegister);
			Debug.Assert(size != InstructionSize.None);
			Debug.Assert(size != InstructionSize.Native);

			//size = BaseMethodCompilerStage.GetInstructionSize(size, destination);

			if (destination.IsCPURegister)
			{
				if (size == InstructionSize.Size8)
					return R_8;

				if (size == InstructionSize.Size16)
					return R_16;

				return R_32;
			}

			if (destination.IsConstant)
			{
				if (size == InstructionSize.Size8)
					return C_8;

				if (size == InstructionSize.Size16)
					return C_16;

				return C_32;
			}

			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="emitter">The emitter.</param>
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			var opCode = ComputeOpCode(node.Size, node.Operand1, node.Operand2);

			if (node.Operand1.IsConstant)
			{
				emitter.Emit(opCode, node.Operand1);
			}
			else
			{
				emitter.Emit(opCode);
			}
		}

		#endregion Methods
	}
}
