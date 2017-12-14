// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Lidt instruction.
	/// </summary>
	public sealed class Lidt : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Lidt"/>.
		/// </summary>
		public Lidt() :
			base(0, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			LidtMemoryConstant(node, emitter);
		}

		private static void LidtMemoryConstant(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsConstant);

			// LIDT – Load Interrupt Descriptor Table Register 0000 1111 : 0000 0001 : modA 011 r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.Append2Bits(Bits.b00)                                          // 2:mod (must not be b11)
				.Append3Bits(Bits.b011)                                         // 3:reg
				.AppendRM(node.Operand1)                                        // 3:r/m (source, always b101)
				.AppendConditionalDisplacement(!node.Operand1.IsConstantZero, node.Operand1)    // 32:displacement value
				.AppendConditionalIntegerValue(node.Operand1.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
