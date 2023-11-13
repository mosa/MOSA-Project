// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadStoreParamR8 : BaseTransform
{
	public LoadStoreParamR8() : base(IR.LoadParamR8, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.StoreParamR8, transform.Window, out var immediate);

		if (previous == null)
			return false;

		if (!immediate && !previous.Operand2.IsDefinedOnce)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.StoreParamR8, transform.Window);

		context.SetInstruction(IR.MoveR8, context.Result, previous.Operand2);
	}
}
