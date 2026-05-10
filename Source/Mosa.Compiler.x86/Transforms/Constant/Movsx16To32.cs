// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Constant;

/// <summary>
/// Movsx16To32
/// </summary>
public sealed class Movsx16To32 : BaseTransform
{
	public static readonly Movsx16To32 Instance = new();

	private Movsx16To32() : base(X86.Movsx16To32, TransformType.Manual | TransformType.Transform)
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
