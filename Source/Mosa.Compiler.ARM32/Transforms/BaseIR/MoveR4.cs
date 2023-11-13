// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// MoveR4
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class MoveR4 : BaseIRTransform
{
	public MoveR4() : base(Framework.IR.MoveR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Mvf, true);
	}
}
