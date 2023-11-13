// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ShiftLeft64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ShiftLeft64 : BaseIRTransform
{
	public ShiftLeft64() : base(Framework.IR.ShiftLeft64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Shl64);
	}
}
