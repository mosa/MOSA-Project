// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class AddCarryOut64CarryNotUsed : BaseTransform
{
	public AddCarryOut64CarryNotUsed() : base(IRInstruction.AddCarryOut64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return !context.Result2.IsUsed;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IRInstruction.Add64, context.Result, context.Operand1);
	}
}
