// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// ZeroExtend32x64
/// </summary>
[Transform("x64.IR")]
public sealed class ZeroExtend32x64 : BaseIRTransform
{
	public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Movzx32To64);
	}
}
