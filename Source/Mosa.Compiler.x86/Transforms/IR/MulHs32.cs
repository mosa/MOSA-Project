// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// MulHs32
/// </summary>
[Transform("x86.IR")]
public sealed class MulHs32 : BaseIRTransform
{
	public MulHs32() : base(IRInstruction.MulHu32, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction2(X86.IMul32, result, v1, operand1, operand2);
	}
}
