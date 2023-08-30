// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ShiftLeft64
/// </summary>
[Transform("x64.IR")]
public sealed class ShiftLeft64 : BaseIRTransform
{
	public ShiftLeft64() : base(IRInstruction.ShiftLeft64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Shl64);
	}
}
