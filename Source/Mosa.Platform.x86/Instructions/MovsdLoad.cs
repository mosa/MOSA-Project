// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovsdLoad instruction.
	/// </summary>
	public sealed class MovsdLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovsdLoad"/>.
		/// </summary>
		public MovsdLoad() :
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
			MovsdMemoryToReg(node, emitter);
		}

		private static void MovsdMemoryToReg(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsRegister);

			var linkreference = node.Operand1.IsLabel || node.Operand1.IsField || node.Operand1.IsSymbol;

			// mem to xmmreg1 1111 0010:0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0010)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendMod(true, node.Operand2)                                 // 2:mod
				.AppendRegister(node.Result.Register)                           // 3:register (destination)
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendConditionalDisplacement(node.Operand2, !node.Operand2.IsConstantZero)    // 8/32:displacement value
				.AppendConditionalIntegerValue(0, linkreference);               // 32:memory

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
