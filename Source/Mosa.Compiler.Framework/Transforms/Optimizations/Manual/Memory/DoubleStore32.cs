﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class DoubleStore32 : BaseTransform
{
	public DoubleStore32() : base(Framework.IR.Store32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		var next = GetNextNodeUntil(context, Framework.IR.Store32, transform.Window, context.Operand1);

		if (next == null)
			return false;

		if (!next.Operand2.IsResolvedConstant)
			return false;

		if (next.Operand1 != context.Operand1)
			return false;

		if (next.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
