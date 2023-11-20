// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// MulSigned32
/// </summary>
public sealed class MulSigned32 : BaseIRTransform
{
	public MulSigned32() : base(IR.MulSigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction2(X86.Mul32, context.Result, v1, context.Operand1, context.Operand2);
	}
}
