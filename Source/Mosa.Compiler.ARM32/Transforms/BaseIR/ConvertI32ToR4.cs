// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ConvertI32ToR4
/// </summary>
[Transform]
public sealed class ConvertI32ToR4 : BaseIRTransform
{
	public ConvertI32ToR4() : base(IR.ConvertI32ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Flt, false);
	}
}
