// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	public static partial class StaticEmitters
	{
		internal static void EmitAdc32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Adc32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAdcConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(AdcConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAdd32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Add32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAddConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(AddConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAddSS(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);

			(emitter as X86CodeEmitter).Emit(AddSS.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAddSD(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);

			(emitter as X86CodeEmitter).Emit(AddSD.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAnd32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(And32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitAndConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(AndConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitBtr32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Btr32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitBtrConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(BtrConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitBts32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsCPURegister);

			(emitter as X86CodeEmitter).Emit(Bts32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitBtsConst32(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand2.IsConstant);

			(emitter as X86CodeEmitter).Emit(BtsConst32.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitSubSS(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);

			(emitter as X86CodeEmitter).Emit(SubSS.LegacyOpcode, node.Result, node.Operand2);
		}

		internal static void EmitSubSD(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1);

			(emitter as X86CodeEmitter).Emit(SubSD.LegacyOpcode, node.Result, node.Operand2);
		}

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

		internal static void EmitCvtsd2ss(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2 == null);

			(emitter as X86CodeEmitter).Emit(Cvtsd2ss.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvtsi2sd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2 == null);

			(emitter as X86CodeEmitter).Emit(Cvtsi2sd.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvtsi2ss(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2 == null);

			(emitter as X86CodeEmitter).Emit(Cvtsi2ss.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvtss2sd(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2 == null);

			(emitter as X86CodeEmitter).Emit(Cvtss2sd.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvttsd2si(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2 == null);

			(emitter as X86CodeEmitter).Emit(Cvttsd2si.LegacyOpcode, node.Result, node.Operand1, null);
		}

		internal static void EmitCvttss2si(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);
			Debug.Assert(node.Operand1.IsCPURegister);
			Debug.Assert(node.Operand2 == null);

			(emitter as X86CodeEmitter).Emit(Cvttss2si.LegacyOpcode, node.Result, node.Operand1, null);
		}
	}
}
