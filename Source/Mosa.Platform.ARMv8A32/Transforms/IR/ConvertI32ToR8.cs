// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// ConvertI32ToR8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class ConvertI32ToR8 : BaseIRTransform
{
	public ConvertI32ToR8() : base(IRInstruction.ConvertI32ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Flt, false);
	}
}
