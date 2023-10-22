// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Move32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Move32 : BaseIRTransform
{
	public Move32() : base(IRInstruction.Move32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Mov, true);
	}
}
