// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// ZeroExtend32x64
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class ZeroExtend32x64 : BaseTransform
{
	public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64, TransformType.Auto | TransformType.Optimization)
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

		var e1 = Operand.CreateConstant(To64(To32(t1)));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}
