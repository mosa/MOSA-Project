// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Invlpg instruction.
	/// </summary>
	public sealed class Invlpg : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Invlpg"/>.
		/// </summary>
		public Invlpg() :
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
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			InvlpgMemory(node, emitter);
		}

		private static void InvlpgMemory(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsConstant);

			var linkreference = node.Operand1.IsLabel || node.Operand1.IsField || node.Operand1.IsSymbol;

			// INVLPG – Invalidate TLB Entry 0000 1111 : 0000 0001 : mod 111 r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.Append2Bits(Bits.b00)                                          // 2:mod (must not be b11)
				.Append3Bits(Bits.b010)                                         // 3:reg
				.AppendRM(node.Operand1)                                        // 3:r/m (source, always b101)
				.AppendConditionalDisplacement(node.Operand1, !node.Operand1.IsConstantZero)    // 32:displacement value
				.AppendConditionalIntegerValue(0, linkreference);               // 32:memory

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
