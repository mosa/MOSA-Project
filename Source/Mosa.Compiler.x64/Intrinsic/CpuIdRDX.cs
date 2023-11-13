// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::CpuIdRDX")]
	private static void CpuIdRDX(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var rbx = transform.PhysicalRegisters.Allocate64(CPURegister.RBX);
		var rcx = transform.PhysicalRegisters.Allocate64(CPURegister.RCX);
		var rdx = transform.PhysicalRegisters.Allocate64(CPURegister.RDX);

		context.SetInstruction(X64.Mov64, rax, operand1);
		context.AppendInstruction(X64.Mov64, rcx, operand2);
		context.AppendInstruction(X64.CpuId, rax, rax, rcx);
		context.AppendInstruction(IR.Gen, rax, rbx, rcx, rdx);
		context.AppendInstruction(X64.Mov64, result, rdx);
	}
}
