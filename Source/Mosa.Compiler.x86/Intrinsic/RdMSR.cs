// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::RdMSR")]
	private static void RdMSR(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		//transform.SplitOperand(result, out Operand resultLow, out Operand resultHigh);

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var edx = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);
		var ecx = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ecx, operand1);
		context.AppendInstruction2(X86.RdMSR, eax, edx, ecx);
		context.AppendInstruction(IR.To64, result, edx, ecx);
	}
}
