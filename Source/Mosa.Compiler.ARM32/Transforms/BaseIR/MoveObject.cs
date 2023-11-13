// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// MoveObject
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class MoveObject : BaseIRTransform
{
	public MoveObject() : base(IR.MoveObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(ARM32.Mov);
	}
}
