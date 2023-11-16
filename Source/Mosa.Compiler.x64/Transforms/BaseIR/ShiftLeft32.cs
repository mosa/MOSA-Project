// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ShiftLeft32
/// </summary>
[Transform]
public sealed class ShiftLeft32 : BaseIRTransform
{
	public ShiftLeft32() : base(IR.ShiftLeft32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Shl32);
	}
}
