// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadStoreParamR8 : BaseTransform
{
	public LoadStoreParamR8() : base(IRInstruction.LoadParamR8, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParamR8, transform.Window, out bool immediate);

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
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParamR8, transform.Window);

		context.SetInstruction(IRInstruction.MoveR8, context.Result, previous.Operand2);
	}
}
