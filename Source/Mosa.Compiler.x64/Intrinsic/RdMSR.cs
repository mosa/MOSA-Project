// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::RdMSR")]
	private static void RdMSR(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transform.SplitOperand(result, out Operand resultLow, out Operand resultHigh);

		var EAX = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var EDX = transform.PhysicalRegisters.Allocate64(CPURegister.RDX);
		var ECX = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);

		context.SetInstruction(X64.Mov64, ECX, operand1);
		context.AppendInstruction2(X64.RdMSR, EAX, EDX, ECX);
		context.AppendInstruction(X64.Mov64, resultLow, EAX);
		context.AppendInstruction(X64.Mov64, resultHigh, EDX);
	}
}
