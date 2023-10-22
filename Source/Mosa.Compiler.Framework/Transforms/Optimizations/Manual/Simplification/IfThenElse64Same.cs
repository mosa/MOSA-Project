// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class IfThenElse64Same : BaseTransform
{
	public IfThenElse64Same() : base(IRInstruction.IfThenElse64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return AreSame(context.Operand2, context.Operand3);
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IRInstruction.Move64, context.Result, context.Operand1);
	}
}
