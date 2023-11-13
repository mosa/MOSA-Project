// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MoveR8
/// </summary>
[Transform("ARM32.IR")]
public sealed class MoveR8 : BaseIRTransform
{
	public MoveR8() : base(Framework.IR.MoveR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Mvf, true);
	}
}
