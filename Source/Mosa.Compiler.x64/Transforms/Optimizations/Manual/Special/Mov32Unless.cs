// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Special;

[Transform("x64.Optimizations.Manual.Special")]
public sealed class Mov32Unless : BaseTransform
{
	public Mov32Unless() : base(X64.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.AreCPURegistersAllocated)
			return false;

		if (!context.Result.IsPhysicalRegister)
			return false;

		if (!context.Operand1.IsPhysicalRegister)
			return false;

		//if (context.Result.Register != CPURegister.ESP)
		//	return false;

		var previous = context.Node.PreviousNonEmpty;

		if (previous == null || previous.Instruction != X64.Mov32)
			return false;

		//if (previous.Result.Register != CPURegister.ESP)
		//	return false;

		if (!previous.Result.IsPhysicalRegister)
			return false;

		if (!previous.Operand1.IsPhysicalRegister)
			return false;

		if (context.Result.Register != previous.Operand1.Register)
			return false;

		if (context.Operand1.Register != previous.Result.Register)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
