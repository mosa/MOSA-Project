// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Special;

public sealed class Mul32Ditto : BaseTransform
{
	public Mul32Ditto() : base(X64.Mul32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (transform.AreCPURegistersAllocated)
			return false;

		if (!context.Result.IsVirtualRegister)
			return false;

		if (!context.Result2.IsVirtualRegister)
			return false;

		var previous = context.Node.PreviousNonEmpty;

		if (previous == null || previous.Instruction != X64.Mul32)
			return false;

		if (!AreSame(context.Operand1, previous.Operand1))
			return false;

		if (!AreSame(context.Operand2, previous.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = context.Node.PreviousNonEmpty;
		var result = context.Result;
		var result2 = context.Result2;

		context.SetInstruction(X64.Mov32, result, previous.Result);
		context.AppendInstruction(X64.Mov32, result2, previous.Result2);
	}
}
