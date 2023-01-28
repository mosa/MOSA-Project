// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// MoveR4
/// </summary>
public sealed class MoveR4 : BaseTransform
{
	public MoveR4() : base(IRInstruction.MoveR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		ARMv8A32TransformHelper.Translate(transform, context, ARMv8A32.Mvf, true);
	}
}
