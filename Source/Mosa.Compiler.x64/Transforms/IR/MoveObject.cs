// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// MoveObject
/// </summary>
[Transform("x64.IR")]
public sealed class MoveObject : BaseIRTransform
{
	public MoveObject() : base(IRInstruction.MoveObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Mov32);
	}
}