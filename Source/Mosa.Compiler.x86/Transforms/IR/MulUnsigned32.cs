// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// MulUnsigned32
/// </summary>
[Transform("x86.IR")]
public sealed class MulUnsigned32 : BaseIRTransform
{
	public MulUnsigned32() : base(IRInstruction.MulUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var v1 = transform.VirtualRegisters.Allocate32();
		context.SetInstruction2(X86.Mul32, v1, context.Result, context.Operand1, context.Operand2);
	}
}
