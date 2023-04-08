// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.FixedRegisters;

/// <summary>
/// Rcr32
/// </summary>
[Transform("x64.FixedRegisters")]
public sealed class Rcr32 : BaseTransform
{
	public Rcr32() : base(X64.Rcr32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;

		var rcx = Operand.CreateCPURegister(transform.I8, CPURegister.RCX);

		context.SetInstruction(X64.Mov64, rcx, operand2);
		context.AppendInstruction(X64.Rcr32, result, operand1, rcx);
	}
}
