﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadParamSignExtend8x64Store8 : BaseTransform
{
	public LoadParamSignExtend8x64Store8() : base(IR.LoadParamSignExtend8x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.StoreParam8, transform.Window, out var immediate);

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
		var previous = GetPreviousNodeUntil(context, IR.StoreParam8, transform.Window);

		context.SetInstruction(IR.SignExtend8x64, context.Result, previous.Operand2);
	}
}
