// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// Xor32Same
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class Xor32Same : BaseTransform
{
	public Xor32Same() : base(Framework.IR.Xor32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!AreSame(context.Operand1, context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.CreateConstant(To32(0));

		context.SetInstruction(Framework.IR.Move32, result, e1);
	}
}
