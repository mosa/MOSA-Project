﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class DoubleStoreParam32 : BaseTransform
{
	public DoubleStoreParam32() : base(Framework.IR.StoreParam32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var next = GetNextNodeUntil(context, Framework.IR.StoreParam32, transform.Window);

		if (next == null)
			return false;

		if (next.Operand1 != context.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
