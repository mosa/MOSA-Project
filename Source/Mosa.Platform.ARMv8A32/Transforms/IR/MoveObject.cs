// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// MoveObject
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class MoveObject : BaseIRTransform
{
	public MoveObject() : base(IRInstruction.MoveObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(ARMv8A32.Mov);
	}
}
