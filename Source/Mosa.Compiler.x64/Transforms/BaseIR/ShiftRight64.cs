// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ShiftRight64
/// </summary>
[Transform]
public sealed class ShiftRight64 : BaseIRTransform
{
	public ShiftRight64() : base(IR.ShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Shr64);
	}
}
