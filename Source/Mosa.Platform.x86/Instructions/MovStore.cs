// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovStore instruction.
	/// </summary>
	public sealed class MovStore : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovLoad"/>.
		/// </summary>
		public MovStore() :
			base(0, 3)
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
			Debug.Assert(node.Operand3.IsRegister);
			Debug.Assert(node.ResultCount == 0);

			var size = BaseMethodCompilerStage.GetInstructionSize(node.Size, node.Operand1);
			var linkreference = node.Operand1.IsLabel || node.Operand1.IsField || node.Operand1.IsSymbol;

			// reg to memory       1000 100w: mod reg r/m
			// immediate to memory 1100 011w: mod 000 r / m : immediate data // todo
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(0x66, size == InstructionSize.Size16)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b100)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width
				.AppendMod(true, node.Operand2)                                 // 2:mod
				.AppendRegister(node.Operand3.Register)                         // 3:register (value)
				.AppendRM(node.Operand1)                                        // 3:r/m (destination)
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
