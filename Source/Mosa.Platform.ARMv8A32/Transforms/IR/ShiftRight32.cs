// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// ShiftRight32
/// </summary>
public sealed class ShiftRight32 : BaseIRTransform
{
	public ShiftRight32() : base(IRInstruction.ShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Lsr, true);
	}
}
