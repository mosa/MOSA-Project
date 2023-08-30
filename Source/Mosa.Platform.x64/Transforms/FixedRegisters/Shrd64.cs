// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// Shrd64
/// </summary>
[Transform("x64.FixedRegisters")]
public sealed class Shrd64 : BaseTransform
{
	public Shrd64() : base(X64.Shrd64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.Operand3.IsConstant)
			return false;

		if (context.Operand3.Register == CPURegister.RCX)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;
		var result = context.Result;

		var rcx = Operand.CreateCPURegister64(CPURegister.RCX);

		context.SetInstruction(X64.Mov64, rcx, operand3);
		context.AppendInstruction(X64.Shrd64, result, operand1, operand2, rcx);
	}
}
