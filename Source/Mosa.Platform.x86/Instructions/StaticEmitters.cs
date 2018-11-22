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

		internal static void EmitMovsdLoad(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// mem to xmmreg1 1111 0010:0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0010)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovsdStore(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsCPURegister);

			// xmmreg1 to mem 1111 0010:0000 1111:0001 0001: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0010)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovssLoad(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// mem to xmmreg1 1111 0011:0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0011)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovssStore(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsCPURegister);

			// xmmreg1 to mem 1111 0011:0000 1111:0001 0001: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0011)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovupsLoad(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// mem to xmmreg1 0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendMod(true, node.Operand2)                                 // 2:mod
				.AppendRegister(node.Result.Register)                           // 3:register (destination)
				.AppendRM(node.Operand1)                                        // 3:r/m (source)

				.AppendConditionalDisplacement(!node.Operand2.IsConstantZero, node.Operand2)    // 8/32:displacement value
				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovupsStore(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(node.ResultCount == 0);
			Debug.Assert(!node.Operand3.IsConstant);

			// xmmreg1 to mem 0000 1111:0001 0001: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovapsLoad(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// mem to xmmreg 1111 0011:0000 1111:0101 1101: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0011)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0101)                                       // 4:opcode
				.AppendNibble(Bits.b1101)                                       // 4:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitJmpFar(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).EmitFarJumpToNextInstruction();
		}

		internal static void EmitMovsxLoad8(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// WARNING: DO NOT USE 0x66 PREFIX WITH THIS INSTRUCTION
			// We currently don't have the ability to load into 16bit registers

			// memory to reg 0000 1111 : 1011 111w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                           // 4:opcode
				.AppendNibble(Bits.b1111)                                           // 4:opcode
				.AppendNibble(Bits.b1011)                                           // 4:opcode
				.Append3Bits(Bits.b111)                                             // 3:opcode
				.AppendWidthBit(false)                                              // 1:width (node.Size != InstructionSize.Size8)
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovsxLoad16(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// WARNING: DO NOT USE 0x66 PREFIX WITH THIS INSTRUCTION
			// We currently don't have the ability to load into 16bit registers

			// memory to reg 0000 1111 : 1011 111w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                           // 4:opcode
				.AppendNibble(Bits.b1111)                                           // 4:opcode
				.AppendNibble(Bits.b1011)                                           // 4:opcode
				.Append3Bits(Bits.b111)                                             // 3:opcode
				.AppendWidthBit(true)                                               // 1:width (node.Size != InstructionSize.Size8)
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovzxLoad8(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// WARNING: DO NOT USE 0x66 PREFIX WITH THIS INSTRUCTION
			// We currently don't have the ability to load into 16bit registers

			// memory to register 0000 1111 : 1011 011w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                           // 4:opcode
				.AppendNibble(Bits.b1111)                                           // 4:opcode
				.AppendNibble(Bits.b1011)                                           // 4:opcode
				.Append3Bits(Bits.b011)                                             // 3:opcode
				.AppendWidthBit(false)                                              // 1:width (node.Size != InstructionSize.Size8)
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovzxLoad16(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// WARNING: DO NOT USE 0x66 PREFIX WITH THIS INSTRUCTION
			// We currently don't have the ability to load into 16bit registers

			// memory to register 0000 1111 : 1011 011w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                           // 4:opcode
				.AppendNibble(Bits.b1111)                                           // 4:opcode
				.AppendNibble(Bits.b1011)                                           // 4:opcode
				.Append3Bits(Bits.b011)                                             // 3:opcode
				.AppendWidthBit(true)                                               // 1:width (node.Size != InstructionSize.Size8)
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovLoadReg(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// memory to reg 1000 101w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b101)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset);
			}

			emitter.Emit(opcode);
		}

		internal static void EmitMovLoadConstantBase(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Result.IsCPURegister);

			//Debug.Assert(node.Operand1.IsLinkerResolved);

			// memory to reg 1000 101w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66) // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                           // 4:opcode
				.Append3Bits(Bits.b101)                                             // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                 // 1:width
				.AppendMod(Bits.b00)                                                // 2:mod
				.AppendRegister(node.Result.Register)                               // 3:register (destination)
				.AppendRM(Bits.b101)                                                // 3:r/m (source)

				.GetPosition(out int patchOffset)
				.AppendConditionalIntegerValue(!node.Operand1.IsResolvedByLinker, node.Operand1.ConstantUnsignedInteger, 0); // 32:memory

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			}

			emitter.Emit(opcode);
		}

		private static void EmitMovLoad(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			if (node.Operand1.IsConstant)
			{
				EmitMovLoadConstantBase(node, emitter, size);
			}
			else
			{
				EmitMovLoadReg(node, emitter, size);
			}
		}

		internal static void EmitMovLoad8(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitMovLoad(node, emitter, InstructionSize.Size8);
		}

		internal static void EmitMovLoad16(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitMovLoad(node, emitter, InstructionSize.Size16);
		}

		internal static void EmitMovLoad32(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitMovLoad(node, emitter, InstructionSize.Size32);
		}

		private static void EmitMovStoreImmediate(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Operand3.IsConstant);

			// immediate to memory	1100 011w: mod 000 r/m : immediate data
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width
				.ModRegRMSIBDisplacement(true, node.Operand1, node.Operand3, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.AppendConditionalIntegerOfSize(!node.Operand3.IsResolvedByLinker, node.Operand3, size) // 8/16/32:immediate
				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand3.IsResolvedByLinker); // 32:memory

			if (node.Operand3.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand3, patchOffset, node.Operand2.ConstantSignedInteger);
			}

			emitter.Emit(opcode);
		}

		private static void EmitMoveStoreConstantBaseImmediate(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Operand1.IsConstant);
			Debug.Assert(node.Operand2.IsResolvedConstant);
			Debug.Assert(node.Operand3.IsConstant);

			// immediate to memory	1100 011w: mod 000 r/m : immediate data
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1100)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width

				.AppendMod(Bits.b00)                                            // 2:mod (00)
				.Append3Bits(Bits.b000)                                         // 3:source (000)
				.AppendRM(node.Operand1)                                        // 3:r/m (destination)

				.GetPosition(out int patchOffset)
				.AppendConditionalPlaceholder32(node.Operand1.IsResolvedByLinker)  // 32:memory
				.AppendConditionalDisplacement(!node.Operand1.IsResolvedByLinker, node.Operand1) // 32:displacement value

				.GetPosition(out int patchOffset2)
				.AppendConditionalPlaceholder32(node.Operand3.IsResolvedByLinker)  // 32:memory
				.AppendConditionalIntegerOfSize(!node.Operand3.IsResolvedByLinker, node.Operand3, size); // 8/16/32:immediate

			if (node.Operand1.IsResolvedByLinker)
			{
				emitter.EmitLink(node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			}

			if (node.Operand3.IsResolvedByLinker)
			{
				Debug.Assert(size == InstructionSize.Size32);

				emitter.EmitLink(node.Operand3, patchOffset2);
			}

			emitter.Emit(opcode);
		}

		private static void EmitMovStoreConstantBase(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);
			Debug.Assert(node.Operand1.IsConstant);

			// reg to memory	1000 100w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b100)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)             // 1:width

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

		private static void EmitMovStoreReg(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(!node.Operand3.IsConstant);

			// reg to memory	1000 100w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.Append3Bits(Bits.b100)                                         // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                  // 1:width

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

		private static void EmitMovStore(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.ResultCount == 0);

			if (node.Operand1.IsConstant && node.Operand3.IsConstant)
			{
				EmitMoveStoreConstantBaseImmediate(node, emitter, size);
			}
			else if (node.Operand3.IsConstant)
			{
				EmitMovStoreImmediate(node, emitter, size);
			}
			else if (node.Operand1.IsConstant && node.Operand3.IsCPURegister)
			{
				EmitMovStoreConstantBase(node, emitter, size);
			}
			else
			{
				EmitMovStoreReg(node, emitter, size);
			}
		}

		internal static void EmitMovStore8(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitMovStore(node, emitter, InstructionSize.Size8);
		}

		internal static void EmitMovStore16(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitMovStore(node, emitter, InstructionSize.Size16);
		}

		internal static void EmitMovStore32(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitMovStore(node, emitter, InstructionSize.Size32);
		}
	}
}
