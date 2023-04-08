// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.FixedRegisters;

/// <summary>
/// Cdq32
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class Cdq32 : BaseTransform
{
	public Cdq32() : base(X86.Cdq32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.Result.IsCPURegister
			&& context.Operand1.IsCPURegister
			&& context.Result.Register == CPURegister.EDX
			&& context.Operand1.Register == CPURegister.EAX)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var result = context.Result;

		var eax = Operand.CreateCPURegister(transform.I4, CPURegister.EAX);
		var edx = Operand.CreateCPURegister(transform.I4, CPURegister.EDX);

		context.SetInstruction(X86.Mov32, eax, operand1);
		context.AppendInstruction(X86.Cdq32, edx, eax);
		context.AppendInstruction(X86.Mov32, result, edx);
	}
}
