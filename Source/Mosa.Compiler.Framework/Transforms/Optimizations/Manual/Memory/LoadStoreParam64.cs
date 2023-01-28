// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadStoreParam64 : BaseTransform
{
	public LoadStoreParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam64, transform.Window, out bool immediate);

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
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam64, transform.Window);

		context.SetInstruction(IRInstruction.Move64, context.Result, previous.Operand2);
	}
}
