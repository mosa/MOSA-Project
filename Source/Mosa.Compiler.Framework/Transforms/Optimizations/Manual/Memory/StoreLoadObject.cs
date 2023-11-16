// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class StoreLoadObject : BaseTransform
{
	public StoreLoadObject() : base(IR.StoreObject, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		var previous = GetPreviousNodeUntil(context, IR.LoadObject, transform.Window, context.Operand3, context.Operand1);

		if (previous == null)
			return false;

		if (!previous.Operand2.IsResolvedConstant)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		if (previous.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
