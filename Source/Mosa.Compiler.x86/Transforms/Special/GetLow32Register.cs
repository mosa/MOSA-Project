// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Special;

[Transform("x86.Special")]
public sealed class GetLow32Register : BaseTransform
{
	public GetLow32Register() : base(Framework.IR.GetLow32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (transform.AreCPURegistersAllocated)
			return false;

		if (!context.Result.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsPhysicalRegister)
			return false;

		if (!(context.Operand1.Register == CPURegister.EBP || context.Operand1.Register == CPURegister.ESP))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(Framework.IR.Move32, context.Result, context.Operand1);
	}
}
