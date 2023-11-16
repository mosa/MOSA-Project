// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ZeroExtend8x64
/// </summary>
[Transform]
public sealed class ZeroExtend8x64 : BaseIRTransform
{
	public ZeroExtend8x64() : base(IR.ZeroExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movzx8To64);
	}
}
