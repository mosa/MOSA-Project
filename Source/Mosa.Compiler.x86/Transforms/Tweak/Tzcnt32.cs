// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// Tzcnt32
/// </summary>
[Transform("x86.Tweak")]
public sealed class Tzcnt32 : BaseTransform
{
	public Tzcnt32() : base(X86.Tzcnt32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstant)
			return false;

		if (context.Operand1.IsCPURegister)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.MoveOperand1ToVirtualRegister(context, X86.Mov32);
	}
}
