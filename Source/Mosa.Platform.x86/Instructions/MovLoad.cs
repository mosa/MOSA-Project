// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
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

		#region Properties

		public override bool ThreeTwoAddressConversion { get { return false; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			if (node.Operand1.IsConstant)
			{
				MovFixedMemoryToReg(node, emitter);
			}
			else
			{
				MovMemoryToReg(node, emitter);
			}
		}

		private static void MovMemoryToReg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// memory to reg 1000 101w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b101)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)                  // 1:width
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		private static void MovFixedMemoryToReg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsLinkerResolved);

			// memory to reg 1000 101w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66) // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                           // 4:opcode
				.Append3Bits(Bits.b101)                                             // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)                 // 1:width
				.AppendMod(Bits.b00)                                                // 2:mod
				.AppendRegister(node.Result.Register)                               // 3:register (destination)
				.AppendRM(Bits.b101)                                                // 3:r/m (source)
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(!node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
