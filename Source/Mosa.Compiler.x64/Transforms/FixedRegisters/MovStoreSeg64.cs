// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// MovStoreSeg64
/// </summary>
[Transform]
public sealed class MovStoreSeg64 : BaseTransform
{
	public MovStoreSeg64() : base(X64.MovStoreSeg64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand1.IsConstant;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Mov64, v1, operand1);
		context.AppendInstruction(X64.MovStoreSeg64, result, v1);
	}
}
