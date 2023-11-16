// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadParamZeroExtend16x32Store16 : BaseTransform
{
	public LoadParamZeroExtend16x32Store16() : base(IR.LoadParamZeroExtend16x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.StoreParam16, transform.Window, out var immediate);

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
		var previous = GetPreviousNodeUntil(context, IR.StoreParam16, transform.Window);

		context.SetInstruction(IR.ZeroExtend16x32, context.Result, previous.Operand2);
	}
}
