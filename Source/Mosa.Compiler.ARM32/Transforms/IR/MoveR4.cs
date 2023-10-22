// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MoveR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class MoveR4 : BaseIRTransform
{
	public MoveR4() : base(IRInstruction.MoveR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Mvf, true);
	}
}
