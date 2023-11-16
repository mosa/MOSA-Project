// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadZeroExtend32x64Store32 : BaseTransform
{
	public LoadZeroExtend32x64Store32() : base(IR.LoadZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		var previous = GetPreviousNodeUntil(context, IR.Store32, transform.Window, out var immediate, context.Operand1);

		if (previous == null)
			return false;

		if (!immediate && !previous.Operand3.IsDefinedOnce)
			return false;

		if (!previous.Operand2.IsResolvedConstant)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		if (previous.Operand2.ConstantUnsigned32 != context.Operand2.ConstantUnsigned32)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.Store32, transform.Window, context.Operand1);

		context.SetInstruction(IR.ZeroExtend32x64, context.Result, previous.Operand3);
	}
}
