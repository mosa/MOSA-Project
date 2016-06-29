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

		#region Properties

		public override bool ThreeTwoAddressConversion { get { return false; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			if (node.Operand1.IsConstant && node.Operand3.IsConstant)
			{
				MovImmediateToFixedMemory(node, emitter);
			}
			else if (node.Operand3.IsConstant || node.Operand3.IsLabel || node.Operand3.IsField || node.Operand3.IsSymbol)
			{
				MovImmediateToMemory(node, emitter);
			}
			else
			{
				MovRegToMemory(node, emitter);
			}
		}

		private static void MovRegToMemory(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsRegister);
			Debug.Assert(node.ResultCount == 0);
			Debug.Assert(!node.Operand3.IsConstant);

			var size = BaseMethodCompilerStage.GetInstructionSize(node.Size, node.Operand1.Type);
			var linkreference = node.Operand1.IsLabel || node.Operand1.IsField || node.Operand1.IsSymbol;

			// reg to memory       1000 100w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(0x66, size == InstructionSize.Size16)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b100)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width
				.ModRegRMSIBDisplacement(node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerValue(0, linkreference);               // 32:memory

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		private static void MovImmediateToMemory(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsConstant);
			Debug.Assert(node.ResultCount == 0);

			var linkreference = node.Operand3.IsLabel || node.Operand3.IsField || node.Operand3.IsSymbol;

			// immediate to memory 1100 011w: mod 000 r/m : immediate data
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(0x66, node.Size == InstructionSize.Size16)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width
				.AppendMod(true, node.Operand2)                                 // 2:mod
				.Append3Bits(Bits.b000)                                         // 3:000
				.AppendRM(node.Operand1)                                        // 3:r/m (destination)
				.AppendConditionalDisplacement(node.Operand2, !node.Operand2.IsConstantZero)      // 8/32:displacement value
				.AppendInteger(node.Operand3, node.Size)                        // 8/16/32:immediate
				.AppendConditionalIntegerValue(0, linkreference);               // 32:memory

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, 24 / 8);
			else
				emitter.Emit(opcode);
		}

		private static void MovImmediateToFixedMemory(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsConstant);
			Debug.Assert(node.Operand2.IsConstantZero);
			Debug.Assert(node.Operand3.IsConstant);
			Debug.Assert(node.ResultCount == 0);

			var linkreference = node.Operand3.IsLabel || node.Operand3.IsField || node.Operand3.IsSymbol;

			// immediate to memory 1100 011w: mod 000 r/m : immediate data
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(0x66, node.Size == InstructionSize.Size16)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width
				.AppendMod(Bits.b00)                                            // 2:mod (000)
				.Append3Bits(Bits.b000)                                         // 3:source (000)
				.AppendRM(node.Operand1)                                        // 3:r/m (destination)
				.AppendConditionalDisplacement(node.Operand1, !linkreference)   // 32:displacement value
				.AppendConditionalIntegerValue(0, linkreference)                // 32:memory
				.AppendInteger(node.Operand3, node.Size);                       // 8/16/32:immediate

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - (int)node.Size) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
