// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// ShiftRight32
/// </summary>
[Transform("x64.IR")]
public sealed class ShiftRight32 : BaseIRTransform
{
	public ShiftRight32() : base(IRInstruction.ShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Shr32);
	}
}
