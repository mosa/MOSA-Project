// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ZeroExtend16x32
/// </summary>
public sealed class ZeroExtend16x32 : BaseTransform
{
	public ZeroExtend16x32() : base(IRInstruction.ZeroExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Movzx16To32);
	}
}
