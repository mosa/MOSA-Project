// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.FixedRegisters;

/// <summary>
/// MovStoreSeg32
/// </summary>
public sealed class MovStoreSeg32 : BaseTransform
{
	public MovStoreSeg32() : base(X64.MovStoreSeg32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Operand1.IsConstant;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var v1 = transform.AllocateVirtualRegister(operand1);

		context.SetInstruction(X64.Mov64, v1, operand1);
		context.AppendInstruction(X64.MovStoreSeg32, result, v1);
	}
}
