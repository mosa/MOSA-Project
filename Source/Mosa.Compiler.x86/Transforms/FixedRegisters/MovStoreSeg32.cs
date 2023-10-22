// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// MovStoreSeg32
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class MovStoreSeg32 : BaseTransform
{
	public MovStoreSeg32() : base(X86.MovStoreSeg32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstant)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var v1 = transform.VirtualRegisters.Allocate(operand1);

		context.SetInstruction(X86.Mov32, v1, operand1);
		context.AppendInstruction(X86.MovStoreSeg32, result, v1);
	}
}
