// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class LoadZeroExtend32x64Store32 : BaseTransform
{
	public LoadZeroExtend32x64Store32() : base(IRInstruction.LoadZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		var previous = GetPreviousNodeUntil(context, IRInstruction.Store32, transform.Window, out bool immediate, context.Operand1);

		if (previous == null)
			return false;

		if (!immediate && !IsSSAForm(previous.Operand3))
			return false;

		if (!previous.Operand2.IsResolvedConstant)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		if (previous.Operand2.ConstantUnsigned32 != context.Operand2.ConstantUnsigned32)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.Store32, transform.Window, context.Operand1);

		context.SetInstruction(IRInstruction.ZeroExtend32x64, context.Result, previous.Operand3);
	}
}
