// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ShiftLeft32
/// </summary>
[Transform("ARM32.IR")]
public sealed class ShiftLeft32 : BaseIRTransform
{
	public ShiftLeft32() : base(Framework.IR.ShiftLeft32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Lsl, true);
	}
}
