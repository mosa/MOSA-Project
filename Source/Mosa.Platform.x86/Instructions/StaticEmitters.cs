// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	public static class StaticEmitters
	{
		internal static void EmitCmpXchgLoad32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);
			Debug.Assert(node.GetOperand(3).IsCPURegister);
			Debug.Assert(node.Result.Register == GeneralPurposeRegister.EAX);
			Debug.Assert(node.Operand1.Register == GeneralPurposeRegister.EAX);
			Debug.Assert(node.ResultCount == 1);

			// Compare EAX with r/m32. If equal, ZF is set and r32 is loaded into r/m32.
			// Else, clear ZF and load r/m32 into EAX.

			// memory, register 0000 1111 : 1011 000w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b1011)                                       // 4:opcode
				.Append3Bits(Bits.b000)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width
				.ModRegRMSIBDisplacement(true, node.GetOperand(3), node.Operand2, node.Operand3) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerValue(node.Operand2.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand2.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand2, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		internal static void EmitComisd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Comisd.LegacyOpcode, node.Operand1, node.Operand2);
		}

		internal static void EmitComiss(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Comiss.LegacyOpcode, node.Operand1, node.Operand2);
		}

		internal static void EmitCvtsd2ss(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Cvtsd2ss.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvtsi2sd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Cvtsi2sd.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvtsi2ss(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Cvtsi2ss.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvtss2sd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Cvtss2sd.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvttsd2si(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Cvttsd2si.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvttss2si(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Cvttss2si.LegacyOpcode, node.Result, node.Operand1, null);
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

		internal static void EmitSub32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Sub32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitSubConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(SubConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitSubsd(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).Emit(Subsd.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitSubss(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).Emit(Subss.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitTest32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Test32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitTestConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).Emit(TestConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitUcomisd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Ucomisd.LegacyOpcode, node.Operand1, node.Operand2);
		}

		internal static void EmitUcomiss(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Ucomiss.LegacyOpcode, node.Operand1, node.Operand2);
		}

		internal static void EmitXchg32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Result2.IsCPURegister);
			Debug.Assert(node.Operand1 == node.Result2);
			Debug.Assert(node.Operand2 == node.Result);

			(emitter as X86CodeEmitter).Emit(Xchg32.LegacyOpcode, node.Result, node.Operand1, node.Operand2);
		}

		internal static void EmitXor32(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).Emit(Xor32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitXorConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			(emitter as X86CodeEmitter).Emit(XorConst32.LegacyOpcode, node.Result, node.Operand2);
		}
	}
}
