// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// SubOverflowOut64
/// </summary>
public sealed class SubOverflowOut64 : BaseTransform
{
	public SubOverflowOut64() : base(Framework.IR.SubOverflowOut64, TransformType.Manual | TransformType.Optimization)
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

		var t1 = context.Operand1.ConstantSigned64;
		var t2 = context.Operand2.ConstantSigned64;

		var e1 = Operand.CreateConstant(t1 - t2);
		var carry = IntegerTwiddling.IsSubSignedOverflow(t1, t2);

		context.SetInstruction(Framework.IR.Move64, result, e1);
		context.AppendInstruction(Framework.IR.Move64, result2, carry ? Operand.Constant64_1 : Operand.Constant64_0);
	}
}
