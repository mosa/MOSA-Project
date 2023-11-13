// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// MulR8
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class MulR8 : BaseIRTransform
{
	public MulR8() : base(IR.MulR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Muf, true);
	}
}
