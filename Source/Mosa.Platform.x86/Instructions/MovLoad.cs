// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovLoad instruction.
	/// </summary>
	public sealed class MovLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovLoad"/>.
		/// </summary>
		public MovLoad() :
			base(1, 2)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsRegister);

			var size = BaseMethodCompilerStage.GetInstructionSize(node.Size, node.Result);
			var linkreference = node.Operand1.IsLabel || node.Operand1.IsField || node.Operand1.IsSymbol;

			// memory to reg 1000 101w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(0x66, size == InstructionSize.Size16)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b101)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width
				.AppendMod(true, node.Operand2)                                 // 2:mod
				.AppendRegister(node.Result.Register)                           // 3:register (destination)
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendConditionalDisplacement(node.Operand2, !node.Operand2.IsConstantZero)      // 8/32:displacement value
				.AppendConditionalIntegerValue(0, linkreference);               // 32:memory

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
