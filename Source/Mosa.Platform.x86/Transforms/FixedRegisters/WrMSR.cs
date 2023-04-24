// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.FixedRegisters;

/// <summary>
/// WrMSR
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class WrMSR : BaseTransform
{
	public WrMSR() : base(X86.WrMSR, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.Operand1.IsCPURegister
			&& context.Operand2.IsCPURegister
			&& context.Operand3.IsCPURegister
			&& context.Operand1.Register == CPURegister.ECX
			&& context.Operand2.Register == CPURegister.EAX
			&& context.Operand3.Register == CPURegister.EDX)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);
		var edx = Operand.CreateCPURegister32(CPURegister.EDX);
		var ecx = Operand.CreateCPURegister32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ecx, operand1);
		context.AppendInstruction(X86.Mov32, eax, operand2);
		context.AppendInstruction(X86.Mov32, edx, operand3);
		context.AppendInstruction(X86.WrMSR, null, ecx, eax, edx);
	}
}
