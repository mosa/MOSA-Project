// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadStoreParamObject : BaseTransform
{
	public LoadStoreParamObject() : base(IR.LoadParamObject, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.StoreParamObject, transform.Window, out var immediate);

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
		var previous = GetPreviousNodeUntil(context, IR.StoreParamObject, transform.Window);

		context.SetInstruction(IR.MoveObject, context.Result, previous.Operand2);
	}
}
