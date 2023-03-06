// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// ConvertI32ToR4
/// </summary>
public sealed class ConvertI32ToR4 : BaseIRTransform
{
	public ConvertI32ToR4() : base(IRInstruction.ConvertI32ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARMv8A32.Flt, false);
	}
}
