// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// MulUnsigned32
/// </summary>
[Transform("x64.IR")]
public sealed class MulUnsigned32 : BaseIRTransform
{
	public MulUnsigned32() : base(IRInstruction.MulUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var v1 = transform.AllocateVirtualRegister32();
		context.SetInstruction2(X64.Mul32, v1, context.Result, context.Operand1, context.Operand2);
	}
}
