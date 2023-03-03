// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Optimizations.Manual.Special;

public sealed class Mov32ConstantReuse : BaseTransform
{
	public Mov32ConstantReuse() : base(X64.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned32 == 0)
			return false;

		if (!context.Result.IsCPURegister)
			return false;

		if (context.Result.Register != CPURegister.RSP)
			return false;

		var previous = context.Node.PreviousNonEmpty;

		if (previous == null || previous.Instruction != X64.Mov32)
			return false;

		if (previous.Result.Register != CPURegister.RSP)
			return false;

		if (!previous.Operand1.IsResolvedConstant)
			return false;

		if (!previous.Result.IsCPURegister)
			return false;

		if (context.Operand1.ConstantUnsigned64 != previous.Operand1.ConstantUnsigned64)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var previous = context.Node.PreviousNonEmpty;

		context.Operand1 = previous.Result;
	}
}
