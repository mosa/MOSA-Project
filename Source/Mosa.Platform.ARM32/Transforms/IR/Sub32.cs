// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// Sub32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Sub32 : BaseIRTransform
{
	public Sub32() : base(IRInstruction.Sub32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Sub, true);
	}
}
