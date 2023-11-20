// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.Special;

public sealed class Mov32ConstantReuse : BaseTransform
{
	public Mov32ConstantReuse() : base(X86.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.AreCPURegistersAllocated)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned32 == 0)
			return false;

		if (!context.Result.IsPhysicalRegister)
			return false;

		if (context.Result.Register != CPURegister.ESP)
			return false;

		var previous = context.Node.PreviousNonEmpty;

		if (previous == null || previous.Instruction != X86.Mov32)
			return false;

		if (previous.Result.Register != CPURegister.ESP)
			return false;

		if (!previous.Operand1.IsResolvedConstant)
			return false;

		if (!previous.Result.IsPhysicalRegister)
			return false;

		if (context.Operand1.ConstantUnsigned64 != previous.Operand1.ConstantUnsigned64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = context.Node.PreviousNonEmpty;

		context.Operand1 = previous.Result;
	}
}
