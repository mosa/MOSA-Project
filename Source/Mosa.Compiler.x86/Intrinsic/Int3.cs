// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Int3")]
	private static void Int3(Context context, Transform transform)
	{
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;
		var operand4 = context.Operand4;

		Helper.FoldOperand1ToConstant(context);

		var operand1 = context.Operand1;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var ebx = transform.PhysicalRegisters.Allocate32(CPURegister.EBX);
		var ecx = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, eax, operand2);
		context.AppendInstruction(X86.Mov32, ebx, operand3);
		context.AppendInstruction(X86.Mov32, ecx, operand4);

		context.AppendInstruction(X86.Int, null, operand1);

		context.AppendInstruction(IR.Use, null, eax);
		context.AppendInstruction(IR.Use, null, ebx);
		context.AppendInstruction(IR.Use, null, ecx);

	}
}
