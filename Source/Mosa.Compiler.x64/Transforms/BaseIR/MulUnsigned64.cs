// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// MulUnsigned64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class MulUnsigned64 : BaseIRTransform
{
	public MulUnsigned64() : base(Framework.IR.MulUnsigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var v1 = transform.VirtualRegisters.Allocate32();
		context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
	}
}
