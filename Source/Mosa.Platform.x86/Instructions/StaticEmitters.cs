// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	public static class StaticEmitters
	{
		internal static void EmitCmpXChgLoad32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);
			Debug.Assert(node.GetOperand(3).IsCPURegister);
			Debug.Assert(node.Result.Register == GeneralPurposeRegister.EAX);
			Debug.Assert(node.Operand1.Register == GeneralPurposeRegister.EAX);

			// Compare EAX with r/m32. If equal, ZF is set and r32 is loaded into r/m32.
			// Else, clear ZF and load r/m32 into EAX.

			// memory, register 0000 1111 : 1011 000w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b1011)                                       // 4:opcode
				.Append3Bits(Bits.b000)                                         // 3:opcode
				.AppendWidthBit(true)                                           // 1:width (node.Size != InstructionSize.Size8)
				.ModRegRMSIBDisplacement(true, node.GetOperand(3), node.Operand2, node.Operand3) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerValue(node.Operand2.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand2.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand2, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

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

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(!node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:displacement

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(!node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:displacement

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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

		internal static void EmitInvlpg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsConstant);

			// INVLPG – Invalidate TLB Entry 0000 1111 : 0000 0001 : mod 111 r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.Append2Bits(Bits.b00)                                          // 2:mod (must not be b11)
				.Append3Bits(Bits.b010)                                         // 3:reg
				.AppendRM(node.Operand1)                                        // 3:r/m (source, always b101)
				.AppendConditionalDisplacement(!node.Operand1.IsConstantZero, node.Operand1)    // 32:displacement value
				.AppendConditionalIntegerValue(node.Operand1.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitLgdt(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsConstant);

			// LGDT – Load Global Descriptor Table Register 0000 1111 : 0000 0001 : modA 010 r / m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.Append2Bits(Bits.b00)                                          // 2:mod (must not be b11)
				.Append3Bits(Bits.b010)                                         // 3:reg
				.AppendRM(node.Operand1)                                        // 3:r/m (source, always b101)
				.AppendConditionalDisplacement(!node.Operand1.IsConstantZero, node.Operand1)    // 32:displacement value
				.AppendConditionalIntegerValue(node.Operand1.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitLidt(InstructionNode node, BaseCodeEmitter emitter)
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

		internal static void EmitMovd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			// reg from mmxreg
			// 0000 1111:0111 1110: 11 mmxreg reg
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0110)                                       // 4:opcode
				.AppendNibble(Bits.b0110)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendBit(node.Result.Register.Width != 128)                   // 1:direction
				.AppendNibble(Bits.b1110)                                       // 4:opcode
				.Append2Bits(Bits.b11)                                          // 2:opcode
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendRegister(node.Result.Register);                          // 3:register (destination)

			emitter.Emit(opcode);
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitPextrd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			// reg from xmmreg, imm8
			// 0110 0110:0000 1111:0011 1010: 0001 0110:11 xmmreg reg: imm8
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0110)                                       // 4:opcode
				.AppendNibble(Bits.b0110)                                       // 4:opcode

				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode

				.AppendNibble(Bits.b0011)                                       // 4:opcode
				.AppendNibble(Bits.b1010)                                       // 4:opcode

				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0110)                                       // 4:opcode

				.Append2Bits(Bits.b11)                                          // 2:opcode
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendRegister(node.Result.Register)                           // 3:register (destination)

				.AppendByteValue((byte)node.Operand2.ConstantUnsignedInteger);  // 8:memory

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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset);  // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitLea32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			//Debug.Assert(node.Size == InstructionSize.Size32);

			// LEA – Load Effective Address 1000 1101 : modA reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.AppendNibble(Bits.b1101)                                       // 3:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerValue(node.Operand1.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitInt(InstructionNode node, BaseCodeEmitter emitter)
		{
			emitter.WriteByte(0xCD);
			emitter.WriteByte((byte)node.Operand1.ConstantUnsignedInteger);
		}

		internal static void EmitMovsx8To32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			// register2 to register1 0000 1111 : 1011 111w : 11 reg1 reg2
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                           // 4:opcode
				.AppendNibble(Bits.b1111)                           // 4:opcode
				.AppendNibble(Bits.b1011)                           // 4:opcode
				.Append3Bits(Bits.b111)                             // 4:opcode
				.AppendWidthBit(false)                              // 1:width (node.Size != InstructionSize.Size8)
				.AppendMod(Bits.b11)                                // 2:mod
				.AppendRegister(node.Result)                        // 3:register (destination)
				.AppendRM(node.Operand1);                           // 3:r/m (source)

			emitter.Emit(opcode);
		}

		internal static void EmitMovsx16To32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			// register2 to register1 0000 1111 : 1011 111w : 11 reg1 reg2
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                           // 4:opcode
				.AppendNibble(Bits.b1111)                           // 4:opcode
				.AppendNibble(Bits.b1011)                           // 4:opcode
				.Append3Bits(Bits.b111)                             // 4:opcode
				.AppendWidthBit(true)                               // 1:width (node.Size != InstructionSize.Size8)
				.AppendMod(Bits.b11)                                // 2:mod
				.AppendRegister(node.Result)                        // 3:register (destination)
				.AppendRM(node.Operand1);                           // 3:r/m (source)

			emitter.Emit(opcode);
		}

		internal static void EmitMovzx8To32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			// register2 to register1 0000 1111 : 1011 011w : 11 reg1 reg2
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                           // 4:opcode
				.AppendNibble(Bits.b1111)                           // 4:opcode
				.AppendNibble(Bits.b1011)                           // 4:opcode
				.Append3Bits(Bits.b011)                             // 4:opcode
				.AppendWidthBit(false)                              // 1:width node.Size != InstructionSize.Size8
				.AppendMod(Bits.b11)                                // 2:mod
				.AppendRegister(node.Result)                        // 3:register (destination)
				.AppendRM(node.Operand1);                           // 3:r/m (source)

			emitter.Emit(opcode);
		}

		internal static void EmitMovzx16To32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			// register2 to register1 0000 1111 : 1011 011w : 11 reg1 reg2
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                           // 4:opcode
				.AppendNibble(Bits.b1111)                           // 4:opcode
				.AppendNibble(Bits.b1011)                           // 4:opcode
				.Append3Bits(Bits.b011)                             // 4:opcode
				.AppendWidthBit(true)                              // 1:width node.Size != InstructionSize.Size8
				.AppendMod(Bits.b11)                                // 2:mod
				.AppendRegister(node.Result)                        // 3:register (destination)
				.AppendRM(node.Operand1);                           // 3:r/m (source)

			emitter.Emit(opcode);
		}

		internal static void EmitPop32(InstructionNode node, BaseCodeEmitter emitter)
		{
			emitter.WriteByte((byte)(0x58 + node.Result.Register.RegisterCode));
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitJmpStatic(InstructionNode node, BaseCodeEmitter emitter)
		{
			emitter.WriteByte(0xE9);
			(emitter as X86CodeEmitter).EmitCallSite(node.Operand1);
		}

		internal static void EmitCallStatic(InstructionNode node, BaseCodeEmitter emitter)
		{
			emitter.WriteByte(0xE8);
			(emitter as X86CodeEmitter).EmitCallSite(node.Operand1);
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
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitMovLoadConstantBase(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsLinkerResolved);

			// memory to reg 1000 101w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(size == InstructionSize.Size16, 0x66) // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                           // 4:opcode
				.Append3Bits(Bits.b101)                                             // 3:opcode
				.AppendWidthBit(size != InstructionSize.Size8)                 // 1:width
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
				.AppendConditionalIntegerOfSize(!node.Operand3.IsLinkerResolved, node.Operand3, size) // 8/16/32:immediate
				.AppendConditionalPatchPlaceholder(node.Operand3.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand3.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand3, patchOffset, node.Operand2.ConstantSignedInteger);
			else
				emitter.Emit(opcode);
		}

		private static void EmitMoveStoreConstantBaseImmediate(InstructionNode node, BaseCodeEmitter emitter, InstructionSize size)
		{
			Debug.Assert(node.Operand1.IsConstant);
			Debug.Assert(node.Operand2.IsResolvedConstant);
			Debug.Assert(node.Operand3.IsResolvedConstant);
			Debug.Assert(node.Operand1.IsConstant);
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

				.AppendConditionalDisplacement(!node.Operand1.IsLinkerResolved, node.Operand1)   // 32:displacement value

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset)  // 32:memory
				.AppendConditionalIntegerOfSize(true, node.Operand3, size);                     // 8/16/32:immediate

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
			{
				emitter.Emit(opcode);
			}
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

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(!node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset, node.Operand2.ConstantSignedInteger);
			else
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
				.AppendWidthBit(size != InstructionSize.Size8)             // 1:width

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:displacement

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
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
