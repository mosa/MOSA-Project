// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Xor32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Xor32 : BaseIRTransform
{
	public Xor32() : base(Framework.IR.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Eor, true);
	}
}
