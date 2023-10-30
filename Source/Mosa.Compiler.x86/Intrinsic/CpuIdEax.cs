// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::CpuIdEAX")]
	private static void CpuIdEAX(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var ebx = transform.PhysicalRegisters.Allocate32(CPURegister.EBX);
		var ecx = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);
		var edx = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);

		context.SetInstruction(X86.Mov32, eax, operand1);
		context.AppendInstruction(X86.Mov32, ecx, operand2);
		context.AppendInstruction(X86.Mov32, ecx, Operand.Constant32_0);
		context.AppendInstruction(X86.CpuId, eax, eax, ecx);
		context.AppendInstruction(IRInstruction.Gen, eax, ebx, ecx, edx);
		context.AppendInstruction(X86.Mov32, result, eax);
	}
}
