// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Sub32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Sub32 : BaseIRTransform
{
	public Sub32() : base(IRInstruction.Sub32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Sub, true);
	}
}
