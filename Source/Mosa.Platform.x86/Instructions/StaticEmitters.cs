// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	public static class StaticEmitters
	{
		internal static void EmitXChgLoad32ConstantBase(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);
			Debug.Assert(node.Operand1.IsConstant);

			// memory with reg : 1000 011w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(true)                                           // 1:width (node.Size != InstructionSize.Size8)

				.AppendMod(Bits.b00)                                            // 2:mod (00)
				.AppendRegister(node.Operand3)                                  // 3:source
				.AppendRegister(Bits.b101)                                      // 3:r/m (101=Fixed Displacement)

				.GetPosition(out int patchOffset)
				.AppendConditionalIntegerValue(!node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger, 0); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitXChgLoad32Reg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);

			// memory with reg : 1000 011w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(true)                                           // 1:width (node.Size != InstructionSize.Size8)

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:displacement

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitXChgLoad32(InstructionNode node, BaseCodeEmitter emitter)
		{
			if (node.Operand1.IsConstant)
			{
				EmitXChgLoad32ConstantBase(node, emitter);
			}
			else
			{
				EmitXChgLoad32Reg(node, emitter);
			}
		}

		internal static void EmitXAddLoad32ConstantBase(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);
			Debug.Assert(node.Operand1.IsConstant);

			// memory, reg: 0000 1111 : 1100 000w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b000)                                         // 3:opcode
				.AppendWidthBit(true)                                           // 1:width (node.Size != InstructionSize.Size8)

				.AppendMod(Bits.b00)                                            // 2:mod (00)
				.AppendRegister(node.Operand3)                                  // 3:source
				.AppendRegister(Bits.b101)                                      // 3:r/m (101=Fixed Displacement)

				.GetPosition(out int patchOffset)
				.AppendConditionalIntegerValue(!node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger, 0); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitXAddLoad32Reg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);

			// memory, reg : 0000 1111 : 1100 000w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b000)                                         // 3:opcode
				.AppendWidthBit(true)                                           // 1:width (node.Size != InstructionSize.Size8)

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:displacement

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitXAddLoad32(InstructionNode node, BaseCodeEmitter emitter)
		{
			if (node.Operand1.IsConstant)
			{
				EmitXAddLoad32ConstantBase(node, emitter);
			}
			else
			{
				EmitXAddLoad32Reg(node, emitter);
			}
		}

		internal static void EmitJmpFar(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).EmitFarJumpToNextInstruction();
		}
	}
}
