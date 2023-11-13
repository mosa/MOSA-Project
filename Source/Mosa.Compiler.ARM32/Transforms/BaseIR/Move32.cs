// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Move32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Move32 : BaseIRTransform
{
	public Move32() : base(Framework.IR.Move32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Mov, true);
	}
}
