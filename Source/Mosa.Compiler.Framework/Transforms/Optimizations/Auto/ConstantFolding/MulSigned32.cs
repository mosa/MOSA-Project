// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// MulSigned32
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSigned32 : BaseTransform
{
	public MulSigned32() : base(IR.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(MulSigned32(ToSigned32(t1), ToSigned32(t2)));

		context.SetInstruction(IR.Move32, result, e1);
	}
}
