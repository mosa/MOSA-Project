﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::FrameCallRetR8")]
	private static void FrameCallRetR8(Context context, Transform transform)
	{
		var result = context.Result;
		var methodAddress = context.Operand1;

		var eax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var edx = transform.PhysicalRegisters.Allocate64(CPURegister.RDX);
		var xmm0 = transform.PhysicalRegisters.Allocate64(CPURegister.XMM0);

		transform.SplitOperand(result, out Operand op0L, out Operand op0H);

		context.SetInstruction(X64.Call, null, methodAddress);
		context.AppendInstruction(IR.Gen, xmm0);

		//context.AppendInstruction(X64.Movdi64ss, eax, xmm0);        // FIXME: X64
		context.AppendInstruction(X64.Pextrd64, edx, xmm0, Operand.Constant64_1);

		context.AppendInstruction(X64.Mov64, op0L, eax);
		context.AppendInstruction(X64.Mov64, op0H, edx);
	}
}
