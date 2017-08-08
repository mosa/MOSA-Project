// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
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
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.ResultCount == 0);

			if (node.Operand1.IsConstant && node.Operand3.IsConstant)
			{
				MovImmediateToFixedMemory(node, emitter);
			}
			else if (node.Operand3.IsConstant)
			{
				MovImmediateToMemory(node, emitter);
			}
			else if (node.Operand1.IsConstant && node.Operand3.IsCPURegister)
			{
				MovRegToFixedMemory(node, emitter);
			}
			else
			{
				MovRegToMemory(node, emitter);
			}
		}

		private static void MovImmediateToMemory(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsConstant);

			int patchOffset;

			// immediate to memory	1100 011w: mod 000 r/m : immediate data
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width
				.ModRegRMSIBDisplacement(true, node.Operand1, node.Operand3, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerOfSize(!node.Operand3.IsLinkerResolved, node.Operand3, node.Size) // 8/16/32:immediate
				.AppendConditionalPatchPlaceholder(node.Operand3.IsLinkerResolved, out patchOffset); // 32:memory

			if (node.Operand3.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand3, patchOffset, node.Operand2.ConstantSignedInteger);
			else
				emitter.Emit(opcode);
		}

		private static void MovImmediateToFixedMemory(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsConstant);
			Debug.Assert(node.Operand2.IsResolvedConstant);
			Debug.Assert(node.Operand3.IsResolvedConstant);

			int patchOffset;

			// immediate to memory	1100 011w: mod 000 r/m : immediate data
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width

				.AppendMod(Bits.b00)                                            // 2:mod (00)
				.Append3Bits(Bits.b000)                                         // 3:source (000)
				.AppendRM(node.Operand1)                                        // 3:r/m (destination)

				.AppendConditionalDisplacement(!node.Operand1.IsLinkerResolved, node.Operand1)   // 32:displacement value

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out patchOffset)  // 32:memory
				.AppendConditionalIntegerOfSize(true, node.Operand3, node.Size);                     // 8/16/32:immediate

			if (node.Operand1.IsLinkerResolved && !node.Operand3.IsLinkerResolved)
			{
				emitter.Emit(opcode, node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			}
			else if (node.Operand1.IsLinkerResolved && node.Operand3.IsLinkerResolved)
			{
				// fixme: trouble!
				throw new NotImplementCompilerException("not here");
			}
			else
				emitter.Emit(opcode);
		}

		private static void MovRegToFixedMemory(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);
			Debug.Assert(node.Operand1.IsConstant);

			int patchOffset;

			// reg to memory	1000 100w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b100)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width

				.AppendMod(Bits.b00)                                            // 2:mod (00)
				.AppendRegister(node.Operand3)                                  // 3:source
				.AppendRegister(Bits.b101)                                      // 3:r/m (101=Fixed Displacement)

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out patchOffset) // 32:memory
				.AppendConditionalIntegerValue(!node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			else
				emitter.Emit(opcode);
		}

		private static void MovRegToMemory(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);

			int patchOffset;

			// reg to memory	1000 100w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit

				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b100)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out patchOffset); // 32:displacement

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
