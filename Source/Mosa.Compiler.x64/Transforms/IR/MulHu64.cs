// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// MulHu64
/// </summary>
[Transform("x64.IR")]
public sealed class MulHu64 : BaseIRTransform
{
	public MulHu64() : base(IRInstruction.MulHu64, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction2(X64.Mul64, result, v1, operand1, operand2);
	}
}
