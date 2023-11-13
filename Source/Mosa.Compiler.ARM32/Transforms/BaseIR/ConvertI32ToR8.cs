// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ConvertI32ToR8
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class ConvertI32ToR8 : BaseIRTransform
{
	public ConvertI32ToR8() : base(Framework.IR.ConvertI32ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Flt, false);
	}
}
