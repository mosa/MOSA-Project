// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

public sealed class SignExtend16x64 : BaseTransform
{
	public SignExtend16x64() : base(IR.SignExtend16x64, TransformType.Auto | TransformType.Optimization, 100)
	{
	}

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

		var e1 = Operand.CreateConstant(SignExtend16x64(ToShort(t1)));

		context.SetInstruction(IR.Move64, result, e1);
	}
}
