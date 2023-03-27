// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.Special;

[Transform("x86.Special")]
public sealed class GetLow32Register : BaseTransform
{
	public GetLow32Register() : base(IRInstruction.GetLow32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (transform.AreCPURegistersAllocated)
			return false;

		if (!context.Result.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsCPURegister)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.Move32, context.Result, context.Operand1);
	}
}
