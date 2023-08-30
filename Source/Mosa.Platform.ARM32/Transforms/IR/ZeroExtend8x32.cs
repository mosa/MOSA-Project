// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ZeroExtend8x32
/// </summary>
[Transform("ARM32.IR")]
public sealed class ZeroExtend8x32 : BaseIRTransform
{
	public ZeroExtend8x32() : base(IRInstruction.ZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Uxtb, false);
	}
}
