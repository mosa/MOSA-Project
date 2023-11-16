// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// MulUnsigned32
/// </summary>
[Transform]
public sealed class MulUnsigned32 : BaseIRTransform
{
	public MulUnsigned32() : base(IR.MulUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var v1 = transform.VirtualRegisters.Allocate32();
		context.SetInstruction2(X64.Mul32, v1, context.Result, context.Operand1, context.Operand2);
	}
}
