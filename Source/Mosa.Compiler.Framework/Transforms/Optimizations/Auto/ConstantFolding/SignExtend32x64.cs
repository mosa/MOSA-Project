// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// SignExtend32x64
/// </summary>
public sealed class SignExtend32x64 : BaseTransform
{
	public SignExtend32x64() : base(IRInstruction.SignExtend32x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		var e1 = transform.CreateConstant(SignExtend32x64(To32(t1)));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}
