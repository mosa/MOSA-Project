// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// IfThenElse64AlwaysTrue
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class IfThenElse64AlwaysTrue : BaseTransform
{
	public IfThenElse64AlwaysTrue() : base(Framework.IR.IfThenElse64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (IsZero(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2;

		context.SetInstruction(Framework.IR.Move64, result, t1);
	}
}
