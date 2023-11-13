// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MulR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class MulR4 : BaseIRTransform
{
	public MulR4() : base(Framework.IR.MulR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Muf, true);
	}
}
