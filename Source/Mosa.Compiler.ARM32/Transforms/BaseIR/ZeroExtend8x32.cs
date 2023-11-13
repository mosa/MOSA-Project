// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ZeroExtend8x32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class ZeroExtend8x32 : BaseIRTransform
{
	public ZeroExtend8x32() : base(Framework.IR.ZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Uxtb, false);
	}
}
