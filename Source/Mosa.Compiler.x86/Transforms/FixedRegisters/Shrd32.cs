// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// Shrd32
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class Shrd32 : BaseTransform
{
	public Shrd32() : base(X86.Shrd32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.Operand3.IsConstant)
			return false;

		if (context.Operand3.Register == CPURegister.ECX)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;
		var result = context.Result;

		var ecx = Operand.CreateCPURegister32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ecx, operand3);
		context.AppendInstruction(X86.Shrd32, result, operand1, operand2, ecx);
	}
}
