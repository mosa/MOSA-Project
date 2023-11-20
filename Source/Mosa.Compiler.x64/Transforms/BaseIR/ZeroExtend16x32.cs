// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ZeroExtend16x32
/// </summary>
public sealed class ZeroExtend16x32 : BaseIRTransform
{
	public ZeroExtend16x32() : base(IR.ZeroExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movzx16To32);
	}
}
