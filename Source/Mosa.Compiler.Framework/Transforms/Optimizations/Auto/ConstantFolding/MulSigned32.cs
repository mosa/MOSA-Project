// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// MulSigned32
/// </summary>
public sealed class MulSigned32 : BaseTransform
{
	public MulSigned32() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = transform.CreateConstant(MulSigned32(ToSigned32(t1), ToSigned32(t2)));

		context.SetInstruction(IRInstruction.Move32, result, e1);
	}
}
