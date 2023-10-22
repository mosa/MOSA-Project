// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MoveObject
/// </summary>
[Transform("ARM32.IR")]
public sealed class MoveObject : BaseIRTransform
{
	public MoveObject() : base(IRInstruction.MoveObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(ARM32.Mov);
	}
}
