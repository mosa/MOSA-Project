// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ShiftRight64
/// </summary>
[Transform("x64.IR")]
public sealed class ShiftRight64 : BaseIRTransform
{
	public ShiftRight64() : base(IRInstruction.ShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Shr64);
	}
}
