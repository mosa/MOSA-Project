// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak;

/// <summary>
/// Shl32
/// </summary>
public sealed class Shl32 : BaseTransform
{
	public Shl32() : base(X86.Shl32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsConstant)
			return false;

		if (context.Operand1.IsCPURegister)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.MoveOperand1ToVirtualRegister(context, X86.Mov32);
	}
}
