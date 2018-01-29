// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;
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
	}
}
