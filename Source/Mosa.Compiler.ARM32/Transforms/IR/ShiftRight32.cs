// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ShiftRight32
/// </summary>
[Transform("ARM32.IR")]
public sealed class ShiftRight32 : BaseIRTransform
{
	public ShiftRight32() : base(IRInstruction.ShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Lsr, true);
	}
}
