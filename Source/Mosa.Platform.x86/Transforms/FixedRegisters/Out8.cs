// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.FixedRegisters;

/// <summary>
/// Out8
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class Out8 : BaseTransform
{
	public Out8() : base(X86.Out8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.Operand1.IsCPURegister
			&& context.Operand2.IsCPURegister
			&& (context.Operand1.Register == CPURegister.EDX || context.Operand1.IsConstant)
			&& context.Operand2.Register == CPURegister.EAX)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var eax = Operand.CreateCPURegister(operand2.Type, CPURegister.EAX);
		var edx = Operand.CreateCPURegister(operand1.Type, CPURegister.EDX);

		context.SetInstruction(X86.Mov32, edx, operand1);
		context.AppendInstruction(X86.Mov32, eax, operand2);
		context.AppendInstruction(X86.Out8, null, edx, eax);
	}
}
