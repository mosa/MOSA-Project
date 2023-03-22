// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadStoreParamR4 : BaseTransform
{
	public LoadStoreParamR4() : base(IRInstruction.LoadParamR4, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParamR4, transform.Window, out var immediate);

		if (previous == null)
			return false;

		if (!immediate && !IsSSAForm(previous.Operand2))
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParamR4, transform.Window);

		context.SetInstruction(IRInstruction.MoveR4, context.Result, previous.Operand2);
	}
}
