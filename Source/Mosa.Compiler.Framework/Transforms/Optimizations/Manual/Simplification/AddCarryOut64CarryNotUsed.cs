// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class AddCarryOut64CarryNotUsed : BaseTransform
{
	public AddCarryOut64CarryNotUsed() : base(IRInstruction.AddCarryOut64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Result2.Uses.Count == 0;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.Add64, context.Result, context.Operand1);
	}
}
