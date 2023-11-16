// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// Blsr32
/// </summary>
[Transform]
public sealed class Blsr32 : BaseTransform
{
	public Blsr32() : base(X86.Blsr32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstant)
			return false;

		if (context.Operand1.IsPhysicalRegister)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.MoveOperand1ToVirtualRegister(context, X86.Mov32);
	}
}
