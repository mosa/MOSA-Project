// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;
using Mosa.Compiler.Framework.Transforms.Compound;

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// MoveCompound
/// </summary>
public sealed class MoveCompound : BaseRuntimeTransform
{
	public MoveCompound() : base(IRInstruction.MoveCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CopyCompound(transform, context, context.Result.Type, transform.StackFrame, context.Result, transform.StackFrame, context.Operand1);
	}
}
