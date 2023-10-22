// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// AddOverflowOut32
/// </summary>
public sealed class AddOverflowOut32 : BaseTransform
{
	public AddOverflowOut32() : base(IRInstruction.AddOverflowOut32, TransformType.Manual | TransformType.Optimization)
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
		var result2 = context.Result2;

		var t1 = context.Operand1.ConstantSigned32;
		var t2 = context.Operand2.ConstantSigned32;

		var e1 = Operand.CreateConstant(t1 + t2);
		var carry = IntegerTwiddling.IsAddSignedOverflow(t1, t2);

		context.SetInstruction(IRInstruction.Move32, result, e1);
		context.AppendInstruction(IRInstruction.Move32, result2, carry ? Operand.Constant32_1 : Operand.Constant32_0);
	}
}
