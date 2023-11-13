// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// MulSigned64
/// </summary>
[Transform("x64.IR")]
public sealed class MulSigned64 : BaseIRTransform
{
	public MulSigned64() : base(Framework.IR.MulSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var v1 = transform.VirtualRegisters.Allocate32();
		context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
	}
}
