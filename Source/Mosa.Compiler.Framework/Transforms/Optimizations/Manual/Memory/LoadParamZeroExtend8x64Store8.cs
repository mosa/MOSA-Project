// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadParamZeroExtend8x64Store8 : BaseTransform
{
	public LoadParamZeroExtend8x64Store8() : base(IRInstruction.LoadParamZeroExtend8x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam8, transform.Window, out var immediate);

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
		var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam8, transform.Window);

		context.SetInstruction(IRInstruction.ZeroExtend8x64, context.Result, previous.Operand2);
	}
}
