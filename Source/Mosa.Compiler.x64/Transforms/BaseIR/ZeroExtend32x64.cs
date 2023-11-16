// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ZeroExtend32x64
/// </summary>
[Transform]
public sealed class ZeroExtend32x64 : BaseIRTransform
{
	public ZeroExtend32x64() : base(IR.ZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movzx32To64);
	}
}
