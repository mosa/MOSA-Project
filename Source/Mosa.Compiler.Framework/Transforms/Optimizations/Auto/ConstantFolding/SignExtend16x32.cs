// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// SignExtend16x32
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class SignExtend16x32 : BaseTransform
{
	public SignExtend16x32() : base(IR.SignExtend16x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		var e1 = Operand.CreateConstant(SignExtend16x32(ToShort(t1)));

		context.SetInstruction(IR.Move32, result, e1);
	}
}
